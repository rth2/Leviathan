using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameLoop : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How fast the gameplay is updated. 1 == 1 second.")]
    [Range(0.1f, Mathf.Infinity)]
    private float tickSpeed = 1.0f;

    [SerializeField]
    [Tooltip("Augments how fast gameplay is updated. The larger the number the faster the gameplay.")]
    [Range(1f, Mathf.Infinity)]
    private float gameSpeed = 1f;

    [SerializeField] const float speedSlow = 2f, speedMedium = 5f, speedFast = 8f;

    [SerializeField] GameObject inGameMenu = null;

    gameSettings settings = null;

    private float currentTick = 0f;
    private bool isPlaying = true;

    public event Action OnNewTickCycle;

    private void Start()
    {
        settings = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<gameSettings>();
        currentTick = tickSpeed;

        SetSpeed();
    }

    void Update()
    {
        if(isPlaying)
        {
            currentTick -= Time.deltaTime * gameSpeed;
            if (currentTick <= 0)
            {
                OnNewTickCycle?.Invoke();   //tell whatever cares that a new cycle has begun
                currentTick = tickSpeed;
            }
        }

    }

    public void SetSpeed()
    {
        if(settings == null) { return; }

        switch(settings.GetStartingSpeed())
        {
            case (gameSettings.gameSpeed.slow):
                gameSpeed = speedSlow;
                break;
            case (gameSettings.gameSpeed.medium):
                gameSpeed = speedMedium;
                break;
            case (gameSettings.gameSpeed.fast):
                gameSpeed = speedFast;
                break;
            default:
                break;
        }
    }

    public void SetIsGamePlaying(bool state)
    {
        isPlaying = state;
    }

    public void HandlePause()
    {
        Debug.Log($"We are paused...");
        isPlaying = false;
        inGameMenu.SetActive(true);

    }
}
