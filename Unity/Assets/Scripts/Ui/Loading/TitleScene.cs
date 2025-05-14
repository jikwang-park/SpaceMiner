using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    [SerializeField]
    private Slider loadingSlider;
    [SerializeField]
    private GameObject startTouchPanel;
    [SerializeField]
    private GameObject loginTouchPanel;
    [SerializeField]
    private GameObject nicknamePanel;
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private Button saveNicknameButton;
    [SerializeField]
    private LocalizationText alarmText;
    [SerializeField]
    private TMP_InputField nicknameInput;
    [SerializeField]
    private TextMeshProUGUI welcomeText;
    
    private string nickname;

    private readonly int NicknameMinLength = 2;
    private readonly int NicknameMaxLength = 8;
    void Start()
    {
        loadingSlider.gameObject.SetActive(false);
        startTouchPanel.gameObject.SetActive(true);
        loginTouchPanel.gameObject.SetActive(false);
    }
    public async void OnClickStartButton()
    {
        startTouchPanel.gameObject.SetActive(false);
        loadingSlider.gameObject.SetActive(true);
        loadingSlider.value = 0f;

        var progress = new Progress<float>(p => {
            loadingSlider.value = p;
        });

        await FirebaseManager.Instance.InitializeAsync(progress);

        loadingSlider.gameObject.SetActive(false);
        loginTouchPanel.gameObject.SetActive(true);
    }
    public void OnClickLoginButton()
    {
        var user = FirebaseManager.Instance.User;
        if (user == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(user.DisplayName))
        {
            PlayGame();
        }
        else
        {
            nicknamePanel.SetActive(true);
        }
    }

    private async void PlayGame()
    {
        welcomeText.text = String.Format(welcomeText.text, FirebaseManager.Instance.User.DisplayName);
        welcomeText.gameObject.SetActive(true);
        loginButton.gameObject.SetActive(false);
        loadingSlider.gameObject.SetActive(true);
        loadingSlider.value = 0f;
        await FirebaseManager.Instance.LoadFromFirebaseAsync();
        StartCoroutine(LoadSceneWithAddressables());
    }

    private IEnumerator LoadSceneWithAddressables()
    {
        var initHandle = Addressables.InitializeAsync();
        while (!initHandle.IsDone)
        {
            loadingSlider.value = initHandle.PercentComplete * 0.5f;
            yield return null;
        }

        var loadHandle = Addressables.LoadSceneAsync("DevelopScene", LoadSceneMode.Single);
        while (!loadHandle.IsDone)
        {
            loadingSlider.value = 0.5f + loadHandle.PercentComplete * 0.5f;
            yield return null;
        }

        loadingSlider.gameObject.SetActive(false);
    }

    public void OnClickCheckNicknameButton()
    {
        string nick = nicknameInput.text;

        bool isCorrect = IsCorrectNickname(nick);

        if(isCorrect)
        {
            nickname = nick;
            alarmText.gameObject.SetActive(!isCorrect);
            saveNicknameButton.interactable = true;
        }
        else
        {
            alarmText.gameObject.SetActive(isCorrect);
        }
    }

    public async void OnClickSaveNicknameButton()
    {
        try
        {
            var user = FirebaseManager.Instance.User;
            var profile = new UserProfile { DisplayName = nickname };
            await user.UpdateUserProfileAsync(profile);

            nicknamePanel.SetActive(false);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"닉네임 저장 실패: {ex}");
        }
    }
    public void OnValueChangedInputNickname()
    {
        if(nicknameInput.text != nickname)
        {
            saveNicknameButton.interactable = false;
        }
    }
    private bool IsCorrectNickname(string nickname)
    {
        if (string.IsNullOrEmpty(nickname))
        {
            return false;
        }

        if (nickname.Length < NicknameMinLength || nickname.Length > NicknameMaxLength)
        {
            return false;
        }

        foreach (char c in nickname)
        {
            if (char.IsWhiteSpace(c))
            {
                return false;
            }
        }

        return true;
    }
}
