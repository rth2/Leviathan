using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{

    [SerializeField] GameLoop gameLoop = null;
    [SerializeField] GameObject gameOverCanvas = null;
    [SerializeField] GameObject victoryImage = null;
    [SerializeField] GameObject defeatImage = null;

    public void HandleVictory()
    {
        if (!gameLoop) { return; }

        gameLoop.SetIsGamePlaying(false);
        gameOverCanvas.SetActive(true);
        victoryImage.SetActive(true);

        //SceneManager.LoadScene(0);
    }

    public void HandleDeath()
    {
        if(!gameLoop) { return; }

        gameLoop.SetIsGamePlaying(false);
        gameOverCanvas.SetActive(true);
        defeatImage.SetActive(true);

        //SceneManager.LoadScene(2);
    }

}
