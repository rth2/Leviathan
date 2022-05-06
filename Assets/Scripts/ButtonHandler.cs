using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{

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

}
