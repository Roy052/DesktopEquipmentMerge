using System.Collections;
using UnityEngine;

public enum BGM
{
    Main
}

public enum SFX
{
    Click,
    Text,
}

public class SoundManager : Singleton
{
    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;

    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    float currentBgmVolume = 0.5f;
    float currentSfxVolume = 0.5f;

    public void Awake()
    {
        soundManager = this;
    }

    public void OnDestroy()
    {
        soundManager = null;
    }

    private void Start()
    {

    }

    public void ChangeBgmVolume(float volume)
    {
        currentBgmVolume = volume;
        bgmAudioSource.volume = volume;
    }

    public void ChangeSfxVolume(float volume)
    {
        currentSfxVolume = volume;
        sfxAudioSource.volume = volume;
    }

    public void PlayBGM(BGM bgm)
    {
        if (bgmClips.Length <= (int)bgm || bgmClips[(int)bgm] == null)
        {
            Debug.LogError($"BGM {bgm} Not Exist");
            return;
        }
        bgmAudioSource.clip = bgmClips[(int)bgm];
        bgmAudioSource.Play();
    }

    public void PlaySFX(SFX sfx)
    {
        if (co_Fade != null)
        {
            StopCoroutine(co_Fade);
            co_Fade = null;
            sfxAudioSource.volume = currentSfxVolume;
        }

        if (sfxClips.Length <= (int)sfx || sfxClips[(int)sfx] == null)
        {
            Debug.LogError($"SFX {sfx} Not Exist");
            return;
        }
        sfxAudioSource.clip = sfxClips[(int)sfx];
        sfxAudioSource.Play();
    }

    Coroutine co_Fade = null;

    public void PlaySFXLoop(SFX sfx)
    {
        if(co_Fade != null)
        {
            StopCoroutine(co_Fade);
            co_Fade = null;
            sfxAudioSource.volume = currentSfxVolume;
        }

        sfxAudioSource.loop = true;
        PlaySFX(sfx);
    }

    public void StopSFX()
    {
        if (co_Fade != null)
        {
            StopCoroutine(co_Fade);
            co_Fade = null;
            sfxAudioSource.volume = currentSfxVolume;
        }

        sfxAudioSource.loop = false;
        co_Fade = StartCoroutine(FadeSFX());
    }

    IEnumerator FadeSFX()
    {
        float time = 0f;
        while(time <= 0.3f)
        {
            sfxAudioSource.volume = Mathf.Lerp(currentSfxVolume, 0, time / 0.3f);
            time += Time.deltaTime;
            yield return null;
        }
        sfxAudioSource.Stop();
        sfxAudioSource.volume = currentSfxVolume;
    }
}
