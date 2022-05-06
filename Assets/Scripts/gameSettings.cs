using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameSettings : MonoBehaviour
{

    public static gameSettings Instance { get; private set; }

    public enum gameSpeed
    {
        slow = 0,
        medium,
        fast
    };

    [SerializeField] gameSpeed startingSpeed = gameSpeed.medium;
    [SerializeField] bool isMusicPlaying = true;
    [SerializeField] float musicVolume = 0.3f;
    [SerializeField] float soundFXVolume = 0.6f;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public gameSpeed GetStartingSpeed()
    {
        return startingSpeed;
    }

    public bool GetIsMusicPlaying()
    {
        return isMusicPlaying;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSoundFXVolume()
    {
        return soundFXVolume;
    }

    public void SetGameSpeed(gameSpeed newSpeed)
    {
        startingSpeed = newSpeed;
    }

    public void SetIsMusicPlaying(bool state)
    {
        isMusicPlaying = state;
    }

    public void SetMusicVolume(float newVolume)
    {
        Mathf.Abs(newVolume);
        Mathf.Clamp(newVolume, 0f, 1f);

        musicVolume = newVolume;
    }

    public void SetSoundFXVolume(float newVolume)
    {
        Mathf.Abs(newVolume);
        Mathf.Clamp(newVolume, 0f, 1f);

        soundFXVolume = newVolume;
    }


}
