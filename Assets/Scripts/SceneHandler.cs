using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    public void HandleVictory()
    {
        SceneManager.LoadScene(1);
    }

    public void HandleDeath()
    {
        SceneManager.LoadScene(0);
    }

}
