using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private SoundDatabase database;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioSource bgmSource;

    private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    private int maxConcurrentSFX = 10;
    private int currentConcurrentSFX = 0;
    private void Start()
    {
        foreach(var data in database.audioDictionary)
        {
            string soundKey = data.Key;
            string addressableKey = data.Value;
            var handle = Addressables.LoadAssetAsync<AudioClip>(addressableKey);
            Addressables.LoadAssetAsync<AudioClip>(addressableKey).Completed += (AsyncOperationHandle<AudioClip> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var audioClip = handle.Result;
                    soundDictionary[soundKey] = audioClip;
                }
                else
                {
                    Debug.LogError("Failed to instantiate prefab with key: ");
                }
            };
        }
    }
    private void Update()
    {
        if(Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                var t = Input.GetTouch(i);
                if (t.phase == TouchPhase.Began)
                {
                    PlaySFX("TouchSFX");
                }
            }
        }
    }
    public void PlaySFX(string sfxKey)
    {
        if(soundDictionary.ContainsKey(sfxKey))
        {
            AudioClip audioClip = soundDictionary[sfxKey];
            if (currentConcurrentSFX < maxConcurrentSFX)
            {
                currentConcurrentSFX++;
                sfxSource.PlayOneShot(audioClip);
                StartCoroutine(WaitAndDecrement(audioClip.length));
            }
        }
    }
    public IEnumerator WaitAndDecrement(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentConcurrentSFX--;
    }

    public void PlayBGM(string bgmKey)
    {
        if (soundDictionary.ContainsKey(bgmKey))
        {
            bgmSource.clip = soundDictionary[bgmKey];
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }
    public void StopBGM()
    {
        bgmSource.Stop();
    }
    public void StopSFX()
    {
        sfxSource.Stop();
    }
}
