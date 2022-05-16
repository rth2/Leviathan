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
        critter = 3,
        teleporter = 4,
        speedBoost = 5,
        movingObstacle = 6
    };

    [Header("Tile Attributes")]
    [SerializeField] TileType tileType;
    [SerializeField] Color neutralColor, foodColor, wallColor, critterColor, obstacleColor = new Color();

    [Header("Board Attributes")]
    [SerializeField] Vector2 posOnBoard = new Vector2();
    [SerializeField] Vector2 indexOnBoard = new Vector2();

    [Header("Renderers")]
    [SerializeField] SpriteRenderer baseSpriteRenderer = null;
    [SerializeField] SpriteRenderer obstacleSpriteRenderer = null;

    bool isSpeedBoost = false;
    bool isTeleporter = false;
    Vector2 moveObstacleDirection = new Vector2();
    Tile teleporterPair = null;

    CreateObjects objectCreator = null;
    [SerializeField] Sprite foodSprite = null;
    [SerializeField] Sprite defaultSprite;


    private void Start()
    {
        objectCreator = GameObject.FindGameObjectWithTag("ObstacleCreator").GetComponent<CreateObjects>();
        SetTileType(tileType);
    }

    #region Getters
    public TileType GetTileType()
    {
        return tileType;
    }

    public bool GetIsSpeedBoost()
    {
        return isSpeedBoost;
    }

    public bool GetIsTeleporter()
    {
        return isTeleporter;
    }

    public Vector2 GetTileIndex()
    {
        return indexOnBoard;
    }

    public Vector2 GetTilePosition()
    {
        return posOnBoard;
    }

    public Vector2 GetMoveableObstacleDirection()
    {
        return moveObstacleDirection;
    }

    public Tile GetTeleporterPair()
    {
        return teleporterPair;
    }

    public Color GetObstacleColor()
    {
        return obstacleSpriteRenderer.color;
    }

    #endregion

    #region Setters
    public void SetIsSpeedBoost(bool isBoost)
    {
        isSpeedBoost = isBoost;
    }

    public void SetIsTeleporter(bool isTeleport)
    {
        isTeleporter = isTeleport;
    }

    public void SetColor(Color newColor)
    {
        //baseSpriteRenderer.color = newColor;
        obstacleSpriteRenderer.color = newColor;
    }

    public void SetTeleporterPair(Tile pairToThisTile)
    {
        teleporterPair = pairToThisTile;
    }

    public void SetMoveableObstacleDirection(Vector2 direction)
    {
        moveObstacleDirection = direction;
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

    /// <summary>
    /// Set the type of the tile from TileType enum. 
    /// </summary>
    /// <param name="type"> The type of the tile you wish to set.</param>
    public void SetTileType(TileType newType)
    {
        tileType = newType;
        ChangeColor();
    }
    #endregion

    /// <summary>
    /// Changes the color of the tile based on the list it is in.
    /// Default color is purple.
    /// </summary>
    private void ChangeColor()
    {
        if (baseSpriteRenderer == null) return;

        switch(tileType)
        {
            case TileType.neutral:
                {
                    baseSpriteRenderer.color = neutralColor;
                    baseSpriteRenderer.sprite = defaultSprite;
                    break;
                }
            case TileType.food:
                {
                    baseSpriteRenderer.color = foodColor;
                    baseSpriteRenderer.sprite = foodSprite;
                    break;
                }
            case TileType.wall:
                {
                    baseSpriteRenderer.color = wallColor;
                    baseSpriteRenderer.sprite = defaultSprite;
                    break;
                }
            case TileType.critter:
                {
                    baseSpriteRenderer.color = critterColor;
                    baseSpriteRenderer.sprite = defaultSprite;
                    break;
                }
            case TileType.teleporter:
                {
                    baseSpriteRenderer.color = Color.clear;
                    obstacleSpriteRenderer.sprite = objectCreator.GetSpriteForTile(TileType.teleporter);
                    break;
                }
            case TileType.speedBoost:
                {
                    baseSpriteRenderer.color = Color.clear;
                    obstacleSpriteRenderer.sprite = objectCreator.GetSpriteForTile(TileType.speedBoost);
                    break;
                }
            case TileType.movingObstacle:
                baseSpriteRenderer.color = obstacleColor;
                obstacleSpriteRenderer.sprite = objectCreator.GetSpriteForTile(TileType.movingObstacle);
                break;
            default:
                {
                    baseSpriteRenderer.color = new Color(1,0,1,1);
                    obstacleSpriteRenderer.sprite = null;
                    break;
                }
        }
    }


}
