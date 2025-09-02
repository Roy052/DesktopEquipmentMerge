using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    const float VolumeDefault = 0.3f;
    const string BGMKey = "BGMKey";
    const string SFXKey = "SFXKey";

    public Slider sliderBGMVolume;
    public Slider sliderSFXVolume;

    float bgmVolumeValue;
    float sfxVolumeValue;

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(BGMKey, bgmVolumeValue);
        PlayerPrefs.SetFloat(SFXKey, sfxVolumeValue);
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(BGMKey, bgmVolumeValue);
        PlayerPrefs.SetFloat(SFXKey, sfxVolumeValue);
    }

    public void Set()
    {
        float bgmVolume = PlayerPrefs.GetFloat(BGMKey, 1);
        float sfxVolume = PlayerPrefs.GetFloat(SFXKey, 1);

        sliderBGMVolume.value = bgmVolume;
        sliderSFXVolume.value = sfxVolume;
    }

    public void ChangeBGMVolume(float value)
    {
        Singleton.soundManager.bgmAudioSource.volume = VolumeDefault * value;
        bgmVolumeValue = value;
    }

    public void ChangeSFXVolume(float value)
    {
        Singleton.soundManager.sfxAudioSource.volume = VolumeDefault * value;
        sfxVolumeValue = value;
    }
}
