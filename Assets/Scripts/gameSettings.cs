using System;
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
    [SerializeField] bool critterGameMode = false;

    [Header("Audio")]
    [SerializeField] bool isMusicPlaying = true, isPlayingSoundFX = true;
    [SerializeField] float musicVolume = 0.3f;
    [SerializeField] float soundFXVolume = 0.6f;
    [SerializeField] AudioSource audioSource = null;

    [Header("Rows and Columns")]
    [SerializeField] int curRows = 25;
    [SerializeField] int curCols = 19;
    [SerializeField] int minRowCol = 4;
    [SerializeField] int maxRowCol = 100;

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

    #region Getters
    public int GetMinRowColAmount()
    {
        return minRowCol;
    }

    public int GetMaxRowColAmount()
    {
        return maxRowCol;
    }

    public int GetNumberOfRows()
    {
        return curRows;
    }

    public int GetNumberOfCols()
    {
        return curCols;
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

    public bool GetIsPlayingSoundFX()
    {
        return isPlayingSoundFX;
    }

    public float GetSoundFXVolume()
    {
        return soundFXVolume;
    }

    public int GetCurSongIndex()
    {
        AudioHandler audioHandler = GetComponent<AudioHandler>();
        return audioHandler.GetCurTrackIndex();
    }

    public int GetStartingSpeedIndex()
    {
        int curSpeedIndex = 0;

        switch(startingSpeed)
        {
            case gameSpeed.slow:
                curSpeedIndex = 0;
                break;
            case gameSpeed.medium:
                curSpeedIndex = 1;
                break;
            case gameSpeed.fast:
                curSpeedIndex = 2;
                break;
            default:
                break;
        }

        return curSpeedIndex;
    }

    public bool GetInCritterGameMode()
    {
        return critterGameMode;
    }

    #endregion

    #region Setters
    public void SetGameSpeed(gameSpeed newSpeed)
    {
        startingSpeed = newSpeed;
    }

    public void SetGameSpeed(int dropdownValue)
    {
        switch (dropdownValue)
        {
            case 0:
                SetGameSpeed(gameSpeed.slow);
                break;
            case 1:
                SetGameSpeed(gameSpeed.medium);
                break;
            case 2:
                SetGameSpeed(gameSpeed.fast);
                break;
            default:
                // SetGameSpeed(gameSpeed.medium);
                break;
        }
    }

    public void SetIsMusicPlaying(bool state)
    {
        isMusicPlaying = state;

        if(!isMusicPlaying)
            audioSource.Pause();
        else
            audioSource.UnPause();

    }

    public void SetIsPlayingSoundFX(bool soundFXState)
    {
        isPlayingSoundFX = soundFXState;

        if (!isPlayingSoundFX)
            SetSoundFXVolume(0f);
        else
            SetSoundFXVolume(0.6f);
    }

    public void SetMusicVolume(float newVolume)
    {
        Mathf.Abs(newVolume);
        Mathf.Clamp(newVolume, 0f, 1f);

        musicVolume = newVolume;

        if(audioSource == null) { return; }
        audioSource.volume = musicVolume;
    }

    public void SetSoundFXVolume(float newVolume)
    {
        Mathf.Abs(newVolume);
        Mathf.Clamp(newVolume, 0f, 1f);

        soundFXVolume = newVolume;

    }

    public void SetNumberOfRows(int newRowAmount)
    {
        if (curRows == newRowAmount) { return; }

        if (newRowAmount < minRowCol)
            curRows = minRowCol;
        else if (newRowAmount > maxRowCol)
            curRows = maxRowCol;
        else
            curRows = newRowAmount;
    }

    public void SetNumberOfCols(int newColAmount)
    {
        if (curCols == newColAmount) { return; }

        if (newColAmount < minRowCol)
            curCols = minRowCol;
        else if (newColAmount > maxRowCol)
            curCols = maxRowCol;
        else
            curCols = newColAmount;
    }

    public void SetInCritterGameMode(bool isCritterGameMode)
    {
        critterGameMode = isCritterGameMode;
    }

    #endregion

}
