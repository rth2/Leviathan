using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] gameSettings settings = null;

    [Header("Music and FX")]
    [SerializeField] AudioClip[] musicTracks = new AudioClip[5];
    [SerializeField] AudioClip[] soundFX = new AudioClip[2];

    public enum AUDIO_MUSIC
    {
        dragonsVillage = 0,
        spookyDungeon = 1,
        spy = 2,
        superHero = 3,
        youAndI = 4

    };

    public enum AUDIO_FX
    {
        critterDie = 0,
        eatFood = 1
    };

    AUDIO_MUSIC curTrack = AUDIO_MUSIC.superHero;
    AudioSource audioSource = null;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null) { return; }

        ChangeMusicTrack(AUDIO_MUSIC.dragonsVillage);
    }

    /// <summary>
    /// Changes the background music if it is different
    /// from the current track.
    /// </summary>
    /// <param name="requestedTrack">Track we want to play.</param>
    public void ChangeMusicTrack(AUDIO_MUSIC requestedTrack)
    {
        if(audioSource == null) { return; }
        if(settings == null) {  return; }
        if(curTrack == requestedTrack) {  return; }

        //get the music volume level from the gameSettings

        switch (requestedTrack)
        {
            case AUDIO_MUSIC.dragonsVillage:
                curTrack = requestedTrack;
                audioSource.clip = musicTracks[0];
                break;
            case AUDIO_MUSIC.spookyDungeon:
                curTrack = requestedTrack;
                audioSource.clip = musicTracks[1];
                break;
            case AUDIO_MUSIC.spy:
                curTrack = requestedTrack;
                audioSource.clip = musicTracks[2];
                break;
            case AUDIO_MUSIC.superHero:
                curTrack = requestedTrack;
                audioSource.clip = musicTracks[3];
                break;
            case AUDIO_MUSIC.youAndI:
                curTrack = requestedTrack;
                audioSource.clip = musicTracks[4];
                break;
            default:
                curTrack = AUDIO_MUSIC.dragonsVillage;
                audioSource.clip = musicTracks[0];
                break;
        }

        audioSource.volume = settings.GetMusicVolume();
        audioSource.Play();
    }

    /// <summary>
    /// Plays the requested sound fx if it exists.
    /// </summary>
    /// <param name="requestedFX">sound FX we want to play.</param>
    public void PlaySoundFX(AUDIO_FX requestedFX)
    {
        if(audioSource == null) { return; }
        if(settings == null) { return; }

        //get the fxVolume from the game settings.

        switch (requestedFX)
        {
            case AUDIO_FX.critterDie:
                audioSource.PlayOneShot(soundFX[0], settings.GetSoundFXVolume());
                break;
            case AUDIO_FX.eatFood:
                audioSource.PlayOneShot(soundFX[1], settings.GetSoundFXVolume());
                break;
            default:
                break;
        }
    }

}
