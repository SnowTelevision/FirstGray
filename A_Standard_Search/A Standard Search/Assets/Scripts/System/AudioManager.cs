using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip buttonSFX;
    public AudioClip playerMoveSFX;
    public AudioClip levelWinSFX;
    public AudioClip levelLoseSFX;
    public AudioClip bgm;
    public AudioSource sfxPlayer;
    public AudioSource bgmPlayer;

    public static float masterVolume; // Overall volume
    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateMasterVolume(float target)
    {
        masterVolume = target;
    }

    public void StartBGM(AudioClip bgm)
    {
        bgmPlayer.volume = masterVolume;
        bgmPlayer.clip = bgm;
        bgmPlayer.UnPause();
    }

    public void PauseBGM()
    {
        bgmPlayer.Pause();
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxPlayer.Stop();
        sfxPlayer.volume = masterVolume;
        sfxPlayer.clip = sfx;
        sfxPlayer.Play();
    }
}
