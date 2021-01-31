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
    public AudioClip endingBGM;
    public AudioSource sfxPlayer;
    public AudioSource bgmPlayer;

    public static float masterVolume; // Overall volume
    public static AudioManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        instance = this;
        masterVolume = 1;
        //Slider volumeSlider = FindObjectOfType<Slider>(true);
        //if (volumeSlider != null)
        //{
        //    masterVolume = volumeSlider.value;
        //}
    }

    public void UpdateMasterVolume(Slider volumeSlider)
    {
        masterVolume = volumeSlider.value;
    }

    public void StartBGM(AudioClip bgm)
    {
        bgmPlayer.volume = masterVolume;
        bgmPlayer.clip = bgm;
        bgmPlayer.Play();
    }

    public void PauseBGM()
    {
        bgmPlayer.Pause();
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxPlayer.Stop();
        float k = BetterRandom.betterRandom(-100, 100) / 1000;// avoid machine gun sound
        sfxPlayer.volume = masterVolume + masterVolume * k;
        sfxPlayer.clip = sfx;
        sfxPlayer.Play();
    }

    public void PlaySFXOneShot(AudioClip sfx)
    {
        sfxPlayer.Stop();
        float k = BetterRandom.betterRandom(-100, 100) / 1000;// avoid machine gun sound
        sfxPlayer.volume = masterVolume + masterVolume * k;
        sfxPlayer.PlayOneShot(sfx);
    }
}
