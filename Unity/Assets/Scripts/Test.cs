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
}
