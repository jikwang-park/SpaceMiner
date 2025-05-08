using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private Slider loadingSlider;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Loading());
    }
    private IEnumerator Loading()
    {
        loadingSlider.value = 0f;

        AsyncOperationHandle<IResourceLocator> handle = Addressables.InitializeAsync();

        while(!handle.IsDone)
        {
            loadingSlider.value = handle.PercentComplete;
            yield return null;
        }

        loadingSlider.value = 1f;
        Addressables.LoadSceneAsync("MainTitleScene", LoadSceneMode.Single);
    }
}
