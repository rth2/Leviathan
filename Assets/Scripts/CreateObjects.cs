using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjects : MonoBehaviour
{
    private const int numberOfObjects = 4;

    [Tooltip("Index Order: Teleporter, SpeedBoost, MovingObstacle, Wall")]
    [SerializeField] Sprite[] obstacleSprites = new Sprite[numberOfObjects];

    [SerializeField] TileTracker tileTracker = null;
    [SerializeField] TileGrid tileGrid = null;
    [SerializeField] Critter critter = null;

    /// <summary>
    /// Sets the sprite image 'on top' of the board.
    /// </summary>
    /// <param name="obstacle">Get sprite for this obstacle type.</param>
    /// <returns></returns>
    public Sprite GetSpriteForTile(Tile_Base.TileType obstacle)
    {
        Sprite requestedSprite = null;
        switch(obstacle)
        {
            case Tile_Base.TileType.teleporter:
                {
                    requestedSprite = obstacleSprites[0];
                    break;
                }
            case Tile_Base.TileType.speedBoost:
                {
                    requestedSprite = obstacleSprites[1];
                    break;
                }
            case Tile_Base.TileType.movingObstacle:
                {
                    requestedSprite = obstacleSprites[2];
                    break;
                }
            case Tile_Base.TileType.wall:
                {
                    requestedSprite = obstacleSprites[3];
                    break;
                }
            default:
                break;
        }
        return requestedSprite;
    }

    public void CreateObstacle(Tile_Base.TileType obstacleType)
    {
        tileTracker.PlaceObjectRandomlyOnGrid(obstacleType);
    }

    private Tile_Base.TileType ChooseRandomObstacleType()
    {
        Tile_Base.TileType obstacleType = Tile_Base.TileType.neutral;

        if (tileTracker == null) { return obstacleType; }



        int randomInt = UnityEngine.Random.Range(0,100);    //returns 0-99

        //we have room to make a pair of teleporters
        if (tileTracker.GetTileList(Tile_Base.TileType.neutral).GetCount() >= 2)
        {
            if (randomInt < 17)
                obstacleType = Tile_Base.TileType.teleporter;
            else if (randomInt >= 17 && randomInt < 50)
                obstacleType = Tile_Base.TileType.movingObstacle;
            else if (randomInt >= 50 && randomInt < 75)
                obstacleType = Tile_Base.TileType.speedBoost;
            else
                obstacleType = Tile_Base.TileType.wall;
        }
        else  // no room for a teleporter pair
        {
            if (randomInt < 40)
                obstacleType = Tile_Base.TileType.movingObstacle;
            else if (randomInt >= 40 && randomInt < 69)
                obstacleType = Tile_Base.TileType.speedBoost;
            else
                obstacleType = Tile_Base.TileType.wall;
        }

        return obstacleType;
    }

    public void CreateRandomObstacle()
    {
        if (tileGrid == null) { return; }
        if (critter == null) { return; }
        if(tileTracker == null) { return; }

        Tile_Base critterHead = tileTracker.GetTileFromList(0, tileTracker.GetTileList(Tile_Base.TileType.critter));

        Vector2 critterHeadIndex = critterHead.GetTileIndex();
        Vector2 directionToMove = critter.GetDirection();

        int tileInFrontOfCritterX = Mathf.FloorToInt(critterHeadIndex.x + directionToMove.x);
        int tileInFrontOfCritterY = Mathf.FloorToInt(critterHeadIndex.y + directionToMove.y);

        Tile_Base tileInFrontOfCritter = tileGrid.GetTileFromTileGrid(tileInFrontOfCritterX, tileInFrontOfCritterY);

        if (tileInFrontOfCritter == null) { return; }

        Tile_Base.TileType obstacleType = Tile_Base.TileType.neutral;

        if(tileInFrontOfCritter.GetTileType() == Tile_Base.TileType.neutral )
        {
            tileTracker.RemoveTileFromList(tileInFrontOfCritter);
            obstacleType = ChooseRandomObstacleType();
            tileTracker.PlaceObjectRandomlyOnGrid(obstacleType);
            tileTracker.AddTileToList(tileInFrontOfCritter);
        }
        else
        {
            obstacleType = ChooseRandomObstacleType();
            tileTracker.PlaceObjectRandomlyOnGrid(obstacleType);
        }

        if (obstacleType == Tile_Base.TileType.teleporter)
            tileTracker.PlaceObjectRandomlyOnGrid(obstacleType);

    }
}
