using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        masterVolume = 1;
    }

    public void UpdateMasterVolume(Slider volumeSlider)
    {
        masterVolume = volumeSlider.value;
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
