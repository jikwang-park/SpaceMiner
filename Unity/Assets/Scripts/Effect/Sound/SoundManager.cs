using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private AudioMixer audioMixer;
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
    public void SetSFXVolume(float volume)
    {
        SaveLoadManager.Data.sfxVolume = volume;
        audioMixer.SetFloat("SFX", Mathf.Log10(SaveLoadManager.Data.sfxVolume) * 20);
    }
    public void SetBGMVolume(float volume)
    {
        SaveLoadManager.Data.bgmVolume = volume;
        audioMixer.SetFloat("BGM", Mathf.Log10(SaveLoadManager.Data.bgmVolume) * 20);
    }
}
