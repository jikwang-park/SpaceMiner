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
    public float sfxVolume;
    public float bgmVolume;
    private int maxConcurrentSFX = 10;
    private int currentConcurrentSFX = 0;
    private void Awake()
    {
        var all = FindObjectsOfType<SoundManager>();
        if (all.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }
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
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        audioMixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20);
        audioMixer.SetFloat("BGM", Mathf.Log10(bgmVolume) * 20);
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
    public void PlayLoopSFX(string sfxKey, bool isLoop)
    {
        if (soundDictionary.ContainsKey(sfxKey))
        {
            AudioClip audioClip = soundDictionary[sfxKey];
            if (isLoop)
            {
                sfxSource.clip = audioClip;
                sfxSource.loop = isLoop;
                sfxSource.Play();
            }
            else
            {
                sfxSource.loop = isLoop;
                sfxSource.Stop();
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
        sfxVolume = volume;
        audioMixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }
    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        audioMixer.SetFloat("BGM", Mathf.Log10(bgmVolume) * 20);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
