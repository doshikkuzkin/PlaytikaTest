using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource backgroundMusic;
    private AudioSource sounds;
    [SerializeField] private AudioClip endGameButtons;
    [SerializeField] private AudioClip settings;
    [SerializeField] private AudioClip asteroid;

    private float musicVolume = 1f;
    private float soundsVolume = 1f;

    private void Awake()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
        soundsVolume = PlayerPrefs.GetFloat("SoundsVolume", 1);
    }

    private void Start()
    {
        backgroundMusic = FindObjectOfType<Music>().GetComponent<AudioSource>();
        sounds = GetComponent<AudioSource>();

        PlayerEvents.OnPlaySound += PlaySound;
        PlayerEvents.OnMusicVolumeChange += f => ChangeVolume(backgroundMusic, f, "MusicVolume");
        PlayerEvents.OnSoundsVolumeChange += f => ChangeVolume(sounds, f, "SoundsVolume");

        sounds.volume = soundsVolume;
        backgroundMusic.volume = musicVolume;
    }

    private void ChangeVolume(AudioSource audioSource, float value, string saveName)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat(saveName, value);
    }

    private void PlaySound(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.EndGameButtons:
                sounds.PlayOneShot(endGameButtons);
                break;
            case SoundType.SettingButton:
                sounds.PlayOneShot(settings);
                break;
            case SoundType.Asteroid:
                sounds.PlayOneShot(asteroid);
                break;
        }
    }
}

public enum SoundType
{
    EndGameButtons,
    SettingButton,
    Asteroid
}
