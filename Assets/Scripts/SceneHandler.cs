using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneHandler : MonoBehaviour
{
    public enum LEVELS
    {
        splashScreen = 0,
        mainMenu = 1,
        critter = 2,
        classic = 3
    }

    [SerializeField] GameLoop gameLoop = null;
    [SerializeField] GameObject mainCanvas = null;
    [SerializeField] GameObject gameOverCanvas = null;
    [SerializeField] GameObject pauseMenuCanvas = null;
    [SerializeField] GameObject victoryImage = null;
    [SerializeField] GameObject defeatImage = null;
    [SerializeField] SaveLoad saveLoad = null;
    [SerializeField] TMPro.TMP_Text scoreText = null;
    [SerializeField] Critter critter = null;
    [SerializeField] Button startSelected = null;

    public void HandleVictory()
    {
        if (!gameLoop) { return; }

        gameLoop.SetIsGamePlaying(false);
        mainCanvas.SetActive(true);
        pauseMenuCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        victoryImage.SetActive(true);

        if (saveLoad == null) { return; }

        saveLoad.OnSave();
        UpdateScoreDisplay();
    }

    public void SetSelected()
    {
        startSelected.Select();
    }

    public void HandleDeath()
    {
        if (!gameLoop) { return; }
        
        gameLoop.SetIsGamePlaying(false);
        mainCanvas.SetActive(true);
        pauseMenuCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        defeatImage.SetActive(true);

        if (saveLoad == null) { return; }
        saveLoad.OnSave();

        UpdateScoreDisplay();
        startSelected.Select();
    }

    public void UpdateScoreDisplay()
    {
        if(saveLoad == null) { return; }
        if(critter == null) { return; }

        int playerScore = critter.GetFoodEaten();
        int highScore = saveLoad.GetCurrentGridHighScore();

        scoreText.text = "Your Score: " + playerScore + "! High Score: " + highScore + "!";
    }

}
