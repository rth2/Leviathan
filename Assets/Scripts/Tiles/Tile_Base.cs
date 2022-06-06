using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class Tile_Base : MonoBehaviour
{

    public enum TileType
    {
        neutral = 0,
        food = 1,
        wall = 2,
        critter = 3,
        teleporter = 4,
        speedBoost = 5,
        movingObstacle = 6
    };

    [Header("Tile Attributes")]
    [SerializeField] protected TileType tileType;

    [Header("Board Attributes")]
    [SerializeField] protected Vector2 posOnBoard = new Vector2();
    [SerializeField] protected Vector2 indexOnBoard = new Vector2();

    [SerializeField] protected SpriteRenderer sr = null;

    protected  bool isSpeedBoost = false;
    protected  bool isTeleporter = false;


    protected virtual void Start()
    {

    }

    #region Getters
    public virtual TileType GetTileType()
    {
        return tileType;
    }

    public virtual Vector2 GetTileIndex()
    {
        return indexOnBoard;
    }

    public virtual Vector2 GetTilePosition()
    {
        return posOnBoard;
    }

    public virtual Color GetColor()
    {
        return sr.color;
    }

    #endregion

    #region Setters

    public virtual void SetTilePosition(int xPos, int yPos)
    {
        posOnBoard.x = xPos;
        posOnBoard.y = yPos;
    }

    public virtual void SetTileIndex(int row, int col)
    {
        indexOnBoard.x = row;
        indexOnBoard.y = col;
    }

    public virtual void SetColor(Color newColor)
    {
        sr.color = newColor;
    }

    public virtual void SetTileType(TileType newType)
    {
        tileType = newType;
    }

    public virtual Vector2 GetMoveableObstacleDirection()
    {
        Vector2 temporary = new Vector2();

        return temporary;

    }

    public virtual bool GetIsSpeedBoost()
    {
        return isSpeedBoost;
    }

    public virtual Tile_Base GetTeleporterPair()
    {
        throw new NotImplementedException();
    }

    public virtual bool GetIsTeleporter()
    {
        return isTeleporter;
    }

    #endregion

}
