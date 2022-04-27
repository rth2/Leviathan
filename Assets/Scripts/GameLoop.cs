using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameLoop : MonoBehaviour
{
    [SerializeField][Tooltip("How fast the gameplay is updated. 1 == 1 second.")][Range(0.1f, Mathf.Infinity)]
    private float tickSpeed = 1.0f;

    private float currentTick = 0f;
    private bool isPaused = false;

    public event Action OnNewTickCycle;

    private void Start()
    {
        currentTick = tickSpeed;
    }

    void Update()
    {
        currentTick -= Time.deltaTime;
        if (currentTick <= 0)
        {
            OnNewTickCycle?.Invoke();   //tell whatever cares that a new cycle has begun
            currentTick = tickSpeed;
        }
    }
}
