using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter : MonoBehaviour
{
    [SerializeField] private int curLength = 2;
    [SerializeField] private Vector2 critterDirection = new Vector2();
    

    private void Awake()
    {
        critterDirection = Vector2.left;
    }

    public int GetLength()
    {
        return curLength;
    }

    public Vector2 GetDirection()
    {
        return critterDirection;
    }

}
