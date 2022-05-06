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


    private float currentTick = 0f;
    private bool isPlaying = true;

    public event Action OnNewTickCycle;

    private void Start()
    {
        currentTick = tickSpeed;
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

    public void SetSpeed(float speed)
    {
        gameSpeed = speed;
    }

    public void SetIsGamePlaying(bool state)
    {
        isPlaying = state;
    }
}
