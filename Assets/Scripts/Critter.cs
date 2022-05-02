using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter : MonoBehaviour
{
    
    [SerializeField] private int curLength = 2;
    [SerializeField] private Vector2 currentDirection = new Vector2();
    

    private void Awake()
    {
        currentDirection = Vector2.left;
    }

    public int GetLength()
    {
        return curLength;
    }

    public Vector2 GetDirection()
    {
        return currentDirection;
    }

    public void AddLength(int amountToAdd)
    {
        if (amountToAdd == 0) { return; }

        Mathf.Abs(amountToAdd);

        curLength += amountToAdd;
    }

    public void SetDirection(Vector2 newDirection)
    {   //x and y are the same so not changing direction
        if(currentDirection.x == newDirection.x && currentDirection.y == newDirection.y) { return; }
        //both values have changed, so direction can change
        if(currentDirection.x != newDirection.x && currentDirection.y != newDirection.y)
        {
            currentDirection = new Vector2(newDirection.x, newDirection.y);
            return;
        }
        //one value has changed the other has not
        if(curLength == 1)  //critter of length 1 can turn any direction
        {
            currentDirection = new Vector2(newDirection.x, newDirection.y);
        }
        return;
    }

    public void SetDirectionTouch(Vector2 newDirection)
    {
        //find distance between where touch happened (new direction x,y) and where the head of the snake is.
    }



}
