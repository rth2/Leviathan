using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter : MonoBehaviour
{
    
    [SerializeField] private int curLength = 2;
    [SerializeField] private Vector2 currentDirection = new Vector2();
    [SerializeField] TileTracker tileTracker = null;

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

    /// <summary>
    /// Sets the direction the critter should go. This is for PC (WASD and Arrow Keys).
    /// </summary>
    /// <param name="newDirection">Requested Direction</param>
    public void SetDirection(Vector2 newDirection)
    {
        //x and y are the same so not changing direction
        if (currentDirection.x == newDirection.x && currentDirection.y == newDirection.y) { return; }
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

    /// <summary>
    /// Sets the direction the critter should go for Mobile. Based on primarey touch location.
    /// </summary>
    /// <param name="touchLocation">Vector2 of the spot touched.</param>
    public void SetDirectionTouch(Vector2 touchLocation)
    {
        if(tileTracker == null) { return; }

        Tile critterHead = tileTracker.GetTileFromList(0, tileTracker.GetTileList(Tile.TileType.critter));

        Vector2 critterHeadPos = new Vector2();
        critterHeadPos = critterHead.GetTilePosition();

        if (critterHeadPos == touchLocation) { return; }

        Vector2 oldDirection = new Vector2();
        oldDirection = currentDirection;

        touchLocation.x = touchLocation.x - critterHeadPos.x;
        touchLocation.y = touchLocation.y - critterHeadPos.y;

        if (touchLocation.x > 0)
            touchLocation.x = 1;
        else if (touchLocation.x < 0)
            touchLocation.x = -1;

        if (touchLocation.y > 0)
            touchLocation.y = -1;
        else if (touchLocation.y < 0)
            touchLocation.y = 1;

        if (touchLocation == currentDirection) { return; }
        //have 1,-1, or 0 for values of x and y.

        if (currentDirection.x != 0)
        {
            currentDirection.x = 0f;
            currentDirection.y = touchLocation.y;
        }
        else if(currentDirection.y != 0)
        {
            currentDirection.x = touchLocation.x;
            currentDirection.y = 0f;
        }

        if(currentDirection == Vector2.zero)
        {
            currentDirection = oldDirection;
        }

    }

    

}
