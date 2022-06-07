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
    private float speedFromFood = 0f;
    private float speedFromBoost = 0f;
    private bool setButton = false;

    [SerializeField] private float augmentedSpeed = 0f;

    [SerializeField] private SceneHandler sceneHandler = null;

    public event Action OnNewTickCycle;

    private void Start()
    {
        settings = gameSettings.Instance;
        currentTick = tickSpeed;

        SetStartingSpeed();
    }

    void Update()
    {
        if(isPlaying)
        {
            CalculateAugmentedSpeed();

            currentTick -= Time.deltaTime * augmentedSpeed;
            currentTick -= Time.deltaTime * gameSpeed;

            if (currentTick <= 0)
            {
                OnNewTickCycle?.Invoke();   //tell whatever cares that a new cycle has begun
                currentTick = tickSpeed;
            }
        }
        else if(setButton)
        {
            sceneHandler.SetSelected();
            setButton = false;
        }

    }

    public void SetStartingSpeed()
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

        setButton = true;
    }

    public bool GetIsGamePlaying()
    {
        return isPlaying;
    }

    private void CalculateAugmentedSpeed()
    {
        augmentedSpeed = speedFromFood + speedFromBoost;
    }

    /// <summary>
    /// Increment critter speed from the eating of food.
    /// </summary>
    /// <param name="speedIncrease">Permanent increase in speed.</param>
    public void AddToSpeedFromFood(float speedIncrease)
    {
        MathF.Abs(speedIncrease);

        speedFromFood += speedIncrease;
    }

    /// <summary>
    /// Used to add speed from a speed boost. 
    /// </summary>
    /// <param name="speedIncrease">Increase speed by this much.</param>
    /// <param name="boostDurationInSeconds">How long this boost will last. 0f == permanent boost.</param>
    public void AddToSpeedFromBoost(float speedIncrease, float boostDurationInSeconds)
    {
        MathF.Abs(speedIncrease);

        speedFromBoost += speedIncrease;

        if (boostDurationInSeconds == 0) { return; }
           
        StartCoroutine(DecreaseSpeedFromBoost(speedIncrease, boostDurationInSeconds));
    }

    /// <summary>
    /// Reduces the boost speed parameter. Negative value adjusted to 0f.
    /// </summary>
    /// <param name="amountToDecrease">Adjust boost speed down this much.</param>
    /// <param name="boostDurationInSeconds">How long to wait before reducing the boost speed.</param>
    /// <returns></returns>
    IEnumerator DecreaseSpeedFromBoost(float amountToDecrease, float boostDurationInSeconds)
    {
        yield return new WaitForSeconds(boostDurationInSeconds);

        speedFromBoost -= amountToDecrease;

        if (speedFromBoost < 0f)
            speedFromBoost = 0f;
    }

    public void HandlePause()
    {
        isPlaying = false;
        inGameMenu.SetActive(true);
    }
}
