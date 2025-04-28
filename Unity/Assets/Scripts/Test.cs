using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
        startButton.interactable = true;
    }
}
