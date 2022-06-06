using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter : MonoBehaviour
{
    [Header("Critter Information")]
    [SerializeField] private int curLength = 1;
    [SerializeField] private Vector2 currentDirection = new Vector2();
    [SerializeField] private Vector2 requestedDirection = new Vector2();


    [Header("Dependencies")]
    [SerializeField] TileTracker tileTracker = null;
    [SerializeField] CreateObjects obstacleCreator = null;

    [SerializeField] private int spawnObjectEveryXFoodEaten = 3;

    private int foodEaten = 0;
    gameSettings settings = null;

    private void Awake()
    {
        currentDirection = Vector2.left;
        requestedDirection = Vector2.left;
    }

    private void Start()
    {
        settings = gameSettings.Instance;
    }

    public void CalculateDirection()
    {
        if(requestedDirection == currentDirection) { return; }

        currentDirection = requestedDirection;
    }

    public void AddLength(int amountToAdd)
    {
        if (amountToAdd == 0) { return; }

        Mathf.Abs(amountToAdd);

        curLength += amountToAdd;
    }

    public void AddFoodEaten()
    {
        foodEaten++;

        if (!settings.GetInCritterGameMode()) { return; }
        if(obstacleCreator == null) { return; }

        if(foodEaten%spawnObjectEveryXFoodEaten == 0)
        {
            obstacleCreator.CreateRandomObstacle();
        }
    }

    #region Getters
    public int GetLength()
    {
        return curLength;
    }

    public Vector2 GetDirection()
    {
        return currentDirection;
    }

    public int GetFoodEaten()
    {
        return foodEaten;
    }

    #endregion


    #region Setters
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
            requestedDirection = new Vector2(newDirection.x, newDirection.y);
            return;
        }
        //one value has changed the other has not
        if(curLength == 1)  //critter of length 1 can turn any direction
        {
            requestedDirection = new Vector2(newDirection.x, newDirection.y);
        }
        return;
    }

    /// <summary>
    /// Sets the direction the critter should go for Mobile. Based on primarey touch location.
    /// </summary>
    /// <param name="touchLocation">Vector2 of the spot touched.</param>
    public void SetDirectionTouch(Vector2 touchLocation)
    {
        if (tileTracker == null) { return; }

        Tile_Base critterHead = tileTracker.GetTileFromList(0, tileTracker.GetTileList(Tile_Base.TileType.critter));

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
            requestedDirection.x = 0f;
            requestedDirection.y = touchLocation.y;
        }
        else if(currentDirection.y != 0)
        {
            requestedDirection.x = touchLocation.x;
            requestedDirection.y = 0f;
        }

        if(currentDirection == Vector2.zero)
        {
            requestedDirection = oldDirection;
        }

    }

    #endregion

}
