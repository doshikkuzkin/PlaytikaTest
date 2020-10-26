using System;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public delegate void OnPlayerDeath();

    public delegate void OnScoreUp(int amount, bool isAsteroid);

    public static event Action<SoundType> OnPlaySound;

    public static event Action<float> OnMusicVolumeChange;
    public static event Action<float> OnSoundsVolumeChange;
    

    private void Awake()
    {
        UpPlayerScore = null;
        PlayerDeath = null;
        OnPlaySound = null;
    }

    public static event OnScoreUp UpPlayerScore;

    //fires event of updating score
    public static void FireScoreUp(int amount, bool isAsteroid)
    {
        UpPlayerScore?.Invoke(amount, isAsteroid);
    }

    public static event OnPlayerDeath PlayerDeath;

    //fires event of player loose
    public static void FirePlayerDeath()
    {
        PlayerDeath?.Invoke();
    }

    public static void PlaySound(SoundType obj)
    {
        OnPlaySound?.Invoke(obj);
    }

    public static void ChangeMusicVolume(float obj)
    {
        OnMusicVolumeChange?.Invoke(obj);
    }

    public static void ChangeSoundsVolume(float obj)
    {
        OnSoundsVolumeChange?.Invoke(obj);
    }
}