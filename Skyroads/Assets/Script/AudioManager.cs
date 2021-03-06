﻿using System;
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

    private void OnValidate()
    {
        if (endGameButtons == null || settings == null || asteroid == null)
        {
            enabled = false;
        }
        else
        {
            enabled = true;
        }
    }

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
        PlayerEvents.OnMusicVolumeChange += ChangeMusicVolume;
        PlayerEvents.OnSoundsVolumeChange += ChangeSoundsVolume;

        sounds.volume = soundsVolume;
        backgroundMusic.volume = musicVolume;
    }

    private void ChangeMusicVolume(float value)
    {
        backgroundMusic.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
    
    private void ChangeSoundsVolume(float value)
    {
        sounds.volume = value;
        PlayerPrefs.SetFloat("SoundsVolume", value);
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

    private void OnDestroy()
    {
        PlayerEvents.OnMusicVolumeChange -= ChangeMusicVolume;
        PlayerEvents.OnSoundsVolumeChange -= ChangeSoundsVolume;
        PlayerEvents.OnPlaySound -= PlaySound;
    }
}

public enum SoundType
{
    EndGameButtons,
    SettingButton,
    Asteroid
}
