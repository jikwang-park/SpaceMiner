using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    private FirebaseManager firebaseManager;
    private void Awake()
    {
        firebaseManager = FirebaseManager.Instance;
    }
    public async void OnClickStartButton()
    {
        await FirebaseManager.Instance.InitializeAsync();
        var handle = Addressables.LoadSceneAsync("DevelopScene", LoadSceneMode.Single);
    }
}
