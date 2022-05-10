using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{

    [SerializeField] GameLoop gameLoop = null;
    [SerializeField] GameObject mainCanvas = null;
    [SerializeField] GameObject gameOverCanvas = null;
    [SerializeField] GameObject victoryImage = null;
    [SerializeField] GameObject defeatImage = null;

    public void HandleVictory()
    {
        if (!gameLoop) { return; }

        gameLoop.SetIsGamePlaying(false);
        mainCanvas.SetActive(true);
        gameOverCanvas.SetActive(true);
        victoryImage.SetActive(true);
    }

    public void HandleDeath()
    {
        if(!gameLoop) { return; }

        gameLoop.SetIsGamePlaying(false);
        mainCanvas.SetActive(true);
        gameOverCanvas.SetActive(true);
        defeatImage.SetActive(true);
    }

}
