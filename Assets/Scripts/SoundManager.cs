using UnityEngine;

public enum BGM
{
    Main
}

public enum SFX
{
    Click,
}

public class SoundManager : Singleton
{
    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;

    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

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
        if (sfxClips.Length <= (int)sfx || sfxClips[(int)sfx] == null)
        {
            Debug.LogError($"SFX {sfx} Not Exist");
            return;
        }
        sfxAudioSource.clip = sfxClips[(int)sfx];
        sfxAudioSource.Play();
    }
}
