using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInScene : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button resetButton;
    [SerializeField]
    private GameObject nicknamePanel;
    [SerializeField]
    private TMP_InputField nicknameInput;
    [SerializeField]
    private Button saveNicknameButton;
    [SerializeField]
    private TextMeshProUGUI alarmText;
    private void Awake()
    {
        startButton.interactable = false;
        resetButton.interactable = false;
        nicknamePanel.SetActive(false);
    }
    private async void Start()
    {
        await FirebaseManager.Instance.InitializeAsync();

        var user = FirebaseManager.Instance.User;
        if (user == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(user.DisplayName))
        {
            startButton.interactable = true;
            resetButton.interactable = true;
        }
        else
        {
            nicknamePanel.SetActive(true);
        }

    }
    public async void OnClickSaveNicknameButton()
    {
        string nick = nicknameInput.text.Trim();
        if (string.IsNullOrEmpty(nick))
        {
            alarmText.text = "닉네임을 입력해주세요.";
            return;
        }

        if (nick.Length < 2 || nick.Length > 6)
        {
            alarmText.text = "닉네임은 2자 이상 6자 이하여야 합니다.";
            return;
        }

        startButton.interactable = false;
        resetButton.interactable = false;
        saveNicknameButton.interactable = false;

        try
        {
            var user = FirebaseManager.Instance.User;
            var profile = new UserProfile { DisplayName = nick };
            await user.UpdateUserProfileAsync(profile);

            nicknamePanel.SetActive(false);
            startButton.interactable = true;
            resetButton.interactable = true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"닉네임 저장 실패: {ex}");
            saveNicknameButton.interactable = true;
        }
    }
    public void OnClickStartButton()
    {
        startButton.interactable = false;
        var handle = Addressables.LoadSceneAsync("DevelopScene", LoadSceneMode.Single);
        FirebaseManager.Instance.UpdateLeaderBoard();
        if (handle.Status == AsyncOperationStatus.Failed)
        {
            startButton.interactable = true;
        }
    }
    public async void OnClickResetButton()
    {
        resetButton.interactable = false;
        try
        {
            await FirebaseManager.Instance.ResetUserDataAsync();
            Debug.Log("Firebase 데이터 초기화 완료");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"데이터 초기화 실패: {ex}");
            resetButton.interactable = true;
        }
    }
}
