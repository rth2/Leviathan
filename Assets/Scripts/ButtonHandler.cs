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
    [SerializeField] Button startSelected = null;

    [Header("Dropdowns")]
    [SerializeField] TMP_Dropdown musicDropdown = null;
    [SerializeField] TMP_Dropdown speedDropdown = null;

    [Header("Sliders")]
    [SerializeField] Slider musicSlider = null;
    [SerializeField] Slider soundFXSlider = null;

    [Header("Toggles")]
    [SerializeField] Toggle musicToggle = null;
    [SerializeField] Toggle soundFXToggle = null;

    [Header("Input Fields")]
    [SerializeField] TMP_InputField rowInputField = null;
    [SerializeField] TMP_InputField colInputField = null;

    AudioHandler audioHandler = null;
    gameSettings settings = null;

    const string WEBSITE_PRIVACY_POLICY = "https://www.robertheslin.com/game-privacy-policies";

    private void Start()
    {
        GameObject gameSettingsObject = GameObject.FindGameObjectWithTag("GameSettings");

        settings = gameSettingsObject.GetComponent<gameSettings>();
        if (settings == null) { return; }
        audioHandler = settings.GetComponent<AudioHandler>();

        startSelected.Select();

        ShowCurrentDropdownOptions();
        ShowCurrentSliderValue();
        ShowCurrentToggles();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene((int)SceneHandler.LEVELS.mainMenu);
    }

    public void OnCritterGameStart()
    {
        if(settings == null) { return; }

        settings.SetInCritterGameMode(true);
        SceneManager.LoadScene((int)SceneHandler.LEVELS.critter);
    }

    public void ClassicGameStart()
    {
        if (settings == null) { return; }

        settings.SetInCritterGameMode(false);
        SceneManager.LoadScene((int)SceneHandler.LEVELS.classic);
    }

    public void OpenWebsitePrivacyPolicy()
    {
        Application.OpenURL(WEBSITE_PRIVACY_POLICY);
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

    public void SetRowCount()
    {
        if(settings == null) { return; }
        if(rowInputField == null) { return; }

        if(!int.TryParse(rowInputField.text, out int newRowAmount))
        {
            settings.SetNumberOfRows(settings.GetMinRowColAmount());
        }
        else
            settings.SetNumberOfRows(newRowAmount);

        rowInputField.text = settings.GetNumberOfRows().ToString();
    }

    public void SetColumnCount()
    {
        if (settings == null) { return; }
        if (colInputField == null) { return; }

        if (!int.TryParse(colInputField.text, out int newColAmount))
        {
            settings.SetNumberOfCols(settings.GetMinRowColAmount());
        }
        else
            settings.SetNumberOfCols(newColAmount);

        colInputField.text = settings.GetNumberOfCols().ToString();
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

    public void ShowCurrentRowColCounts()
    {
        if(settings == null) { return; }
        if(rowInputField == null) { return; }
        if(colInputField == null) { return; }

        rowInputField.text = settings.GetNumberOfRows().ToString();
        colInputField.text = settings.GetNumberOfCols().ToString();

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
        gameLoop.SetStartingSpeed();
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
