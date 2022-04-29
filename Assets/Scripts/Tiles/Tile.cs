using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        neutral = 0,
        food = 1,
        wall = 2,
        critter = 3
    };

    [SerializeField] TileType tileType;
    [SerializeField] Color neutralColor, foodColor, wallColor, critterColor = new Color();

    [SerializeField] Vector2 posOnBoard = new Vector2();
    [SerializeField] Vector2 indexOnBoard = new Vector2();

    SpriteRenderer sr = null;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetTileType(tileType);
    }

    public TileType GetTileType()
    {
        return tileType;
    }

    public void SetTilePosition(int xPos, int yPos)
    {
        posOnBoard.x = xPos;
        posOnBoard.y = yPos;
    }

    public void SetTileIndex(int row, int col)
    {
        indexOnBoard.x = row;
        indexOnBoard.y = col;
    }

    public Vector2 GetTileIndex()
    {
        return indexOnBoard;
    }

    public Vector2 GetTilePosition()
    {
        return posOnBoard;
    }

    /// <summary>
    /// Set the type of the tile from TileType enum. 
    /// </summary>
    /// <param name="type"> The type of the tile you wish to set.</param>
    public void SetTileType(TileType newType)
    {
        tileType = newType;
        ChangeColor();
    }

    /// <summary>
    /// Changes the color of the tile based on the list it is in.
    /// Default color is purple.
    /// </summary>
    private void ChangeColor()
    {
        if (sr == null) return;

        switch(tileType)
        {
            case TileType.neutral:
                {
                    sr.color = neutralColor;
                    break;
                }
            case TileType.food:
                {
                    sr.color = foodColor;
                    break;
                }
            case TileType.wall:
                {
                    sr.color = wallColor;
                    break;
                }
            case TileType.critter:
                {
                    sr.color = critterColor;
                    break;
                }
            default:
                {
                    sr.color = new Color(1,0,1,1);
                    break;
                }
        }
    }


}
