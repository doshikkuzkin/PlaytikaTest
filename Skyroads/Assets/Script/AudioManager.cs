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

    private void Start()
    {
        backgroundMusic = FindObjectOfType<Music>().GetComponent<AudioSource>();
        sounds = GetComponent<AudioSource>();

        PlayerEvents.OnPlaySound += PlaySound;
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
