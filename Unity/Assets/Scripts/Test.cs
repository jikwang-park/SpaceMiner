using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button resetButton;
    public async void OnClickStartButton()
    {
        startButton.interactable = false;
        await FirebaseManager.Instance.InitializeAsync();
        var handle = Addressables.LoadSceneAsync("DevelopScene", LoadSceneMode.Single);
        if(handle.Status == AsyncOperationStatus.Failed)
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
