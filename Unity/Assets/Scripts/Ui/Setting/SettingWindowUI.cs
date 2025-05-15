using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindowUI : MonoBehaviour
{
    [SerializeField]
    private Slider sfxVolumeSlider;
    [SerializeField]
    private Slider bgmVolumeSlider;
    private void Start()
    {
        sfxVolumeSlider.value = SoundManager.Instance.sfxVolume;
        bgmVolumeSlider.value = SoundManager.Instance.bgmVolume;
    }
    public void OnClickSaveButton()
    {
        SaveLoadManager.SaveGame();
    }
    public async void OnClickResetGameDataButton()
    {
        await FirebaseManager.Instance.ResetUserDataAsync();
    }
    public void OnValueChangedSFXVolumeSlider()
    {
        SoundManager.Instance.SetSFXVolume(sfxVolumeSlider.value);
    }
    public void OnValueChangedBGMVolumeSlider()
    {
        SoundManager.Instance.SetBGMVolume(bgmVolumeSlider.value);
    }
}
