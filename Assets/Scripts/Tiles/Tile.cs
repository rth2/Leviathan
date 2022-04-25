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
        snake = 3
    };

    [SerializeField] TileType tileType;
    [SerializeField] Color neutralColor, foodColor, wallColor, snakeColor = new Color();
    //[SerializeField] TileTracker tileTracker = null;


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

    /// <summary>
    /// Set the type of the tile from TileType enum. 
    /// </summary>
    /// <param name="type"> The type of the tile you wish to set.</param>
    public void SetTileType(TileType type)
    {
        tileType = type;
        ChangeColor();
    }


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
            case TileType.snake:
                {
                    sr.color = snakeColor;
                    break;
                }
            default:
                {
                    sr.color = new Color(0,0,0);
                    break;
                }
        }
    }


}
