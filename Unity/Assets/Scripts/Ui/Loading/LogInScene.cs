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
            alarmText.text = "�г����� �Է����ּ���.";
            return;
        }

        if (nick.Length < 2 || nick.Length > 6)
        {
            alarmText.text = "�г����� 2�� �̻� 6�� ���Ͽ��� �մϴ�.";
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
            Debug.LogError($"�г��� ���� ����: {ex}");
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
            Debug.Log("Firebase ������ �ʱ�ȭ �Ϸ�");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"������ �ʱ�ȭ ����: {ex}");
            resetButton.interactable = true;
        }
    }
}
