using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] gameSettings settings = null;

    [Header("Music and FX")]
    [SerializeField] AudioClip[] musicTracks = new AudioClip[5];
    [SerializeField] AudioClip[] soundFX = new AudioClip[4];

    public enum AUDIO_FX
    {
        critterDie = 0,
        eatFood = 1,
        teleporter = 2,
        speedBoost = 3
    };

    AudioSource musicAudioSource = null;

    [SerializeField] int curTrack = 3;


    private void Start()
    {
        musicAudioSource = GetComponent<AudioSource>();

        if(musicAudioSource == null) { return; }

        ChangeMusicTrack(curTrack);
    }

    /// <summary>
    /// Gets the index for the current background music.
    /// </summary>
    /// <returns>CurTrack</returns>
    public int GetCurTrackIndex()
    {
        return curTrack;
    }

    /// <summary>
    /// Changes the background music if it is different
    /// from the current track.
    /// </summary>
    /// <param name="dropdownValue">Track we want to play based on TMP dropdown.</param>
    public void ChangeMusicTrack(int dropdownValue)
    {
        if(musicAudioSource == null) { return; }
        if(settings == null) {  return; }
        if(curTrack == dropdownValue) {  return; }

        switch (dropdownValue)
        {
            case 0:
                curTrack = dropdownValue;
                musicAudioSource.clip = musicTracks[0];
                break;
            case 1:
                curTrack = dropdownValue;
                musicAudioSource.clip = musicTracks[1];
                break;
            case 2:
                curTrack = dropdownValue;
                musicAudioSource.clip = musicTracks[2];
                break;
            case 3:
                curTrack = dropdownValue;
                musicAudioSource.clip = musicTracks[3];
                break;
            case 4:
                curTrack = dropdownValue;
                musicAudioSource.clip = musicTracks[4];
                break;
            default:
                curTrack = 0;
                musicAudioSource.clip = musicTracks[0];
                break;
        }

        musicAudioSource.volume = settings.GetMusicVolume();
        musicAudioSource.Play();

        if(settings.GetIsMusicPlaying() == false)
        {
            musicAudioSource.Pause();
        }

    }

    /// <summary>
    /// Plays the requested sound fx if it exists.
    /// </summary>
    /// <param name="requestedFX">Sound FX we want to play.</param>
    /// <param name="FXaudioSource">Audio source to play the fx. So volume can be changed without
    ///                                 having the bg music and fx volume linked.</param>
    public void PlaySoundFX(AUDIO_FX requestedFX, AudioSource FXaudioSource)
    {
        if(FXaudioSource == null) { return; }
        if(settings == null) { return; }

        switch (requestedFX)
        {
            case AUDIO_FX.critterDie:
                FXaudioSource.PlayOneShot(soundFX[0], settings.GetSoundFXVolume());
                break;
            case AUDIO_FX.eatFood:
                FXaudioSource.PlayOneShot(soundFX[1], settings.GetSoundFXVolume());
                break;
            case AUDIO_FX.teleporter:
                FXaudioSource.PlayOneShot(soundFX[2], settings.GetSoundFXVolume());
                break;
            case AUDIO_FX.speedBoost:
                FXaudioSource.PlayOneShot(soundFX[3], settings.GetSoundFXVolume());
                break;
            default:
                break;
        }
    }
}
