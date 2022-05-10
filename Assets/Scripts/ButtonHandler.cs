using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] GameLoop gameLoop = null;

    [Header("Dropdowns")]
    [SerializeField] TMP_Dropdown musicDropdown = null;
    [SerializeField] TMP_Dropdown speedDropdown = null;

    [Header("Sliders")]
    [SerializeField] Slider musicSlider = null;
    [SerializeField] Slider soundFXSlider = null;

    [Header("Toggles")]
    [SerializeField] Toggle musicToggle = null;
    [SerializeField] Toggle soundFXToggle = null;

    AudioHandler audioHandler = null;
    gameSettings settings = null;
    

    private void Start()
    {
        GameObject gameSettingsObject = GameObject.FindGameObjectWithTag("GameSettings");

        settings = gameSettingsObject.GetComponent<gameSettings>();
        if (settings == null) { return; }
        audioHandler = settings.GetComponent<AudioHandler>();

        ShowCurrentDropdownOptions();
        ShowCurrentSliderValue();
        ShowCurrentToggles();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnCritterGameStart()
    {
        SceneManager.LoadScene(1);
    }

    public void ClassicGameStart()
    {
        SceneManager.LoadScene(2);
    }

    public void Exit()
    {
        Debug.Log($"Will quit in executable.");
        Application.Quit();
    }

    public void RestartGame()
    {
        Scene curScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(curScene.buildIndex);
    }

    public void ResumeGame()
    {
        if(gameLoop == null) { return; }

        gameLoop.SetIsGamePlaying(true);

    }

    private void ShowCurrentDropdownOptions()
    {
        if(settings == null) { return; }
        if(musicDropdown == null) { return; }
        if(speedDropdown == null) { return; }

        musicDropdown.SetValueWithoutNotify(settings.GetCurSongIndex());
        speedDropdown.SetValueWithoutNotify(settings.GetStartingSpeedIndex());
    }

    private void ShowCurrentSliderValue()
    {
        if(settings == null) { return; }
        if(musicSlider == null) { return; }
        if(soundFXSlider == null) { return; }

        musicSlider.SetValueWithoutNotify(settings.GetMusicVolume());
        soundFXSlider.SetValueWithoutNotify(settings.GetSoundFXVolume());
    }

    private void ShowCurrentToggles()
    {
        if(settings == null) { return; }
        if(musicToggle == null) { return; }
        if(soundFXToggle == null) { return; }

        musicToggle.SetIsOnWithoutNotify(settings.GetIsMusicPlaying());
        soundFXToggle.SetIsOnWithoutNotify(settings.GetIsPlayingSoundFX());

    }

    public void ChangeBackgroundMusic( int optionValue)
    {
        if(audioHandler == null) { return; }
        audioHandler.ChangeMusicTrack(optionValue);
    }

    public void ChangeGameSpeed(int optionValue)
    {
        if(settings == null) { return; }
        settings.SetGameSpeed(optionValue);

        if (gameLoop == null) { return; }
        gameLoop.SetSpeed();
    }

    public void ChangeMusicVolume(float newVolume)
    {
        if(settings == null) { return; }

        settings.SetMusicVolume(newVolume);
    }

    public void ChangeSoundFXVolume(float newVolume)
    {
        if (settings == null) { return; }

        settings.SetSoundFXVolume(newVolume);
    }

    public void ToggleMusic()
    {
        if (settings == null) { return; }
        bool musicState = settings.GetIsMusicPlaying();

        musicState = !musicState;

        settings.SetIsMusicPlaying(musicState);
    }

    public void ToggleSoundFX()
    {
        if(settings == null) { return; }

        bool soundFXState = settings.GetIsPlayingSoundFX();

        soundFXState = !soundFXState;

        settings.SetIsPlayingSoundFX(soundFXState);

        ShowCurrentSliderValue();
    }

}
