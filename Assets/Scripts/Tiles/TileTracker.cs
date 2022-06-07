using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTracker : MonoBehaviour
{
    [Header("Tile Attributes")]
    [SerializeField] float boostSpeedIncrease = 5f;
    [SerializeField] float boostDurationInSeconds = 2f;

    [Header("Object dependencies")]
    [SerializeField] GameLoop gameLoop = null;
    [SerializeField] Critter critter = null;
    [SerializeField] SceneHandler sceneHandler = null;
    [SerializeField] TileGrid tileGrid = null;
    [SerializeField] ClassicCreateFood foodCreator = null;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource = null;

    gameSettings settings = null;
    AudioHandler audioHandler = null;
    TileList neutralList, critterList, foodList, wallList, teleporterList, speedBoostList, movingObstacleList;

    bool useBody01 = true;

    /// <summary>
    /// Sets up the different lists of tiles.
    /// </summary>
    private void Awake()
    {
        neutralList = new TileList();
        critterList = new TileList();
        foodList = new TileList();
        wallList = new TileList();
        teleporterList = new TileList();
        speedBoostList = new TileList();
        movingObstacleList = new TileList();

        neutralList.SetTileTracker();
        critterList.SetTileTracker();
        foodList.SetTileTracker();
        wallList.SetTileTracker();
        teleporterList.SetTileTracker();
        speedBoostList.SetTileTracker();
        movingObstacleList.SetTileTracker();

        neutralList.SetTileType(Tile_Base.TileType.neutral);
        critterList.SetTileType(Tile_Base.TileType.critter);
        foodList.SetTileType(Tile_Base.TileType.food);
        wallList.SetTileType(Tile_Base.TileType.wall);
        teleporterList.SetTileType(Tile_Base.TileType.teleporter);
        speedBoostList.SetTileType(Tile_Base.TileType.speedBoost);
        movingObstacleList.SetTileType(Tile_Base.TileType.movingObstacle);
    }

    private void Start()
    {
        if (gameLoop == null) { return; }
        gameLoop.OnNewTickCycle += MoveCritter;
        gameLoop.OnNewTickCycle += MoveMoveableObstacles;

        settings = gameSettings.Instance;
        if(settings == null) { return; }

        audioHandler = settings.GetComponent<AudioHandler>();
    }

    public TileList GetTileList(Tile_Base.TileType listType)
    {
        TileList listToGet = null;

        switch(listType)
        {
            case Tile_Base.TileType.critter:
                listToGet = critterList;
                break;
            case Tile_Base.TileType.food:
                listToGet = foodList;
                break;
            case Tile_Base.TileType.neutral:
                listToGet = neutralList;
                break;
            case Tile_Base.TileType.wall:
                listToGet = wallList;
                break;
            case Tile_Base.TileType.speedBoost:
                listToGet = speedBoostList;
                break;
            case Tile_Base.TileType.teleporter:
                listToGet = teleporterList;
                break;
            case Tile_Base.TileType.movingObstacle:
                listToGet = movingObstacleList;
                break;
            default:
                break;
        }

        return listToGet;
    }

    public void MoveMoveableObstacles()
    {
        if (tileGrid == null) { return; }
        if(movingObstacleList.GetCount() == 0) { return; }

        for (int i = 0; i < movingObstacleList.GetCount(); i++)
        {
            Tile_Base obstacleTile = GetTileFromList(i, movingObstacleList);

            Vector2 obstacleTileIndex = obstacleTile.GetTileIndex();

            float moveToTileX = obstacleTile.GetMoveableObstacleDirection().x + obstacleTileIndex.x;
            float moveToTileY = obstacleTile.GetMoveableObstacleDirection().y + obstacleTileIndex.y;

            Tile_Base moveToThisTile = tileGrid.GetTileFromTileGrid((int)moveToTileX, (int)moveToTileY);

            switch(moveToThisTile.GetTileType())
            {
                case Tile_Base.TileType.neutral:
                    tileGrid.ChangeTile((int)moveToThisTile.GetTileIndex().x, (int)moveToThisTile.GetTileIndex().y, Tile_Base.TileType.movingObstacle);
                    break;
                case Tile_Base.TileType.speedBoost:
                    tileGrid.ChangeTile((int)moveToThisTile.GetTileIndex().x, (int)moveToThisTile.GetTileIndex().y, Tile_Base.TileType.movingObstacle);
                    break;
                default:  //anything else and this should break
                    break;
            }

            //should the spot i'm leaving actually be a speed boost tile
            if (speedBoostList.GetCount() > 0)
            {
                for (int j = 0; j < speedBoostList.GetCount(); j++)
                {
                    Vector2 oldVector = new Vector2(obstacleTileIndex.x, obstacleTileIndex.y);
                    if (speedBoostList.GetTile(j).GetTileIndex() == oldVector)
                    {
                        Tile_Base tile = speedBoostList.GetTile(j);
                        tileGrid.ChangeTile((int)obstacleTileIndex.x, (int)obstacleTileIndex.y, Tile_Base.TileType.speedBoost);
                        return;
                    }
                }
            }

                tileGrid.ChangeTile((int)obstacleTileIndex.x, (int)obstacleTileIndex.y, Tile_Base.TileType.neutral);
        }

    }

    public void MoveCritter()
    {

        if(tileGrid == null) { return; }
        if(critter == null) { return; }

        Tile_Base critterHead = GetTileFromList(0, critterList);

        Vector2 critterHeadIndex = critterHead.GetTileIndex();
        critter.CalculateDirection();
        Vector2 directionToMove = critter.GetDirection();

        int newCritterHeadIndexX = Mathf.FloorToInt(critterHeadIndex.x + directionToMove.x);
        int newCritterHeadIndexY = Mathf.FloorToInt(critterHeadIndex.y + directionToMove.y);

        Tile_Base newCritterHead = tileGrid.GetTileFromTileGrid(newCritterHeadIndexX, newCritterHeadIndexY);
        if (newCritterHead == null) { return; }

        switch(newCritterHead.GetTileType())
        {
            case Tile_Base.TileType.neutral:
                {
                    MoveCritterTail();
                    tileGrid.ChangeTile(newCritterHeadIndexX, newCritterHeadIndexY, Tile_Base.TileType.critter);
                    break;
                }
            case Tile_Base.TileType.food:
                {
                    tileGrid.ChangeTile(newCritterHeadIndexX, newCritterHeadIndexY, Tile_Base.TileType.critter);

                    if (audioHandler)
                    {
                        audioHandler.PlaySoundFX(AudioHandler.AUDIO_FX.eatFood, audioSource);
                    }

                    critter.AddLength(1);

                    if (foodList.GetCount() == 0)
                    {
                        tileGrid.PlaceFoodOnGrid();
                    }

                    critter.AddFoodEaten();

                    if (settings == null) { return; }
                    if (!settings.GetInCritterGameMode()) { return; }
                    if (gameLoop == null) { return; }
                    if (foodCreator == null) { return; }

                    gameLoop.AddToSpeedFromFood(foodCreator.GetIncreaseSpeedAmount());
                    break;
                }
            case Tile_Base.TileType.teleporter:
                {
                    audioHandler.PlaySoundFX(AudioHandler.AUDIO_FX.teleporter, audioSource);
                    newCritterHead = newCritterHead.GetTeleporterPair();

                    MoveCritterTail();

                    tileGrid.ChangeTile((int)newCritterHead.GetTileIndex().x, (int)newCritterHead.GetTileIndex().y, Tile_Base.TileType.critter);
                    //MoveCritter();
                    break;
                }
            case Tile_Base.TileType.speedBoost:
                {
                    if(gameLoop == null) { return; }

                    audioHandler.PlaySoundFX(AudioHandler.AUDIO_FX.speedBoost, audioSource);

                    MoveCritterTail();

                    tileGrid.ChangeTile(newCritterHeadIndexX, newCritterHeadIndexY, Tile_Base.TileType.critter);

                    //play a sound
                    gameLoop.AddToSpeedFromBoost(boostSpeedIncrease, boostDurationInSeconds);
                    break;
                }
            default:    //the rest are walls, critter, or moving obstacles
                {
                    if (audioHandler)
                    {
                        audioHandler.PlaySoundFX(AudioHandler.AUDIO_FX.critterDie, audioSource);
                    }
                    sceneHandler.HandleDeath();
                    break;
                }
        }
    }

    private void SetCritterSprites()
    {
        if(critterList.GetCount() == 0) { return; }

        if(critterList.GetCount() > 1)
        {
            critterList.GetTile(1).GetComponent<Tile_Critter>().SetBodySprite(useBody01);
            useBody01 = !useBody01;
        }
    }

    private void MoveCritterTail()
    {
        Tile_Base oldCritterTail = GetTileFromList(critterList.GetCount() - 1, critterList);
        if (oldCritterTail == null) { return; }
        Vector2 critterTailIndex = oldCritterTail.GetTileIndex();

        int critterTailIndexX = Mathf.FloorToInt(critterTailIndex.x);
        int critterTailIndexY = Mathf.FloorToInt(critterTailIndex.y);

        Vector2 oldIndex = oldCritterTail.GetTileIndex();

        if (speedBoostList.GetCount() > 0)
        {
            for(int i = 0; i < speedBoostList.GetCount(); i++)
            {
                if(speedBoostList.GetTile(i).GetTileIndex() == oldIndex)
                {
                    Tile_Base tile = speedBoostList.GetTile(i);
                    tileGrid.ChangeTile(critterTailIndexX, critterTailIndexY, Tile_Base.TileType.speedBoost);
                    return;
                }
            }
        }
        if (teleporterList.GetCount() > 0)
        {
            for (int i = 0; i < teleporterList.GetCount(); i++)
            {
                if (teleporterList.GetTile(i).GetTileIndex() == oldIndex)
                {
                    Tile_Base tile = teleporterList.GetTile(i);
                    tileGrid.ChangeTile(critterTailIndexX, critterTailIndexY, Tile_Base.TileType.teleporter);
                    return;
                }
            }
        }
        tileGrid.ChangeTile(critterTailIndexX, critterTailIndexY, Tile_Base.TileType.neutral);
    }

    public void AddTileToList( Tile_Base tile)
    {
        switch (tile.GetTileType()){
            case Tile_Base.TileType.neutral:
                neutralList.AddTileToList(tile);
                break;
            case Tile_Base.TileType.critter:
                critterList.AddTileToList(tile);
                SetCritterSprites();
                break;
            case Tile_Base.TileType.food:
                foodList.AddTileToList(tile);
                break;
            case Tile_Base.TileType.wall:
                wallList.AddTileToList(tile);
                break;
            case Tile_Base.TileType.movingObstacle:
                movingObstacleList.AddTileToList(tile);
                tile.GetComponent<Tile_MovingObstacle>().SetMoveableObstacleDirection(Vector2.right);
                break;
            case Tile_Base.TileType.teleporter:
                teleporterList.AddTileToList(tile);
                break;
            case Tile_Base.TileType.speedBoost:
                speedBoostList.AddTileToList(tile);
                break;
            default:
                break;
        }
    }

    public void RemoveTileFromList(Tile_Base tile)
    {
        switch (tile.GetTileType())
        {
            case Tile_Base.TileType.neutral:
                neutralList.RemoveTileFromList(tile);
                break;
            case Tile_Base.TileType.critter:
                critterList.RemoveTileFromList(tile);
                break;
            case Tile_Base.TileType.food:
                foodList.RemoveTileFromList(tile);
                if (neutralList.GetCount() == 0 && foodList.GetCount() == 0)
                {
                    sceneHandler.HandleVictory();
                }
                break;
            case Tile_Base.TileType.wall:
                wallList.RemoveTileFromList(tile);
                break;
            case Tile_Base.TileType.movingObstacle:
                movingObstacleList.RemoveTileFromList(tile);
                break;
            case Tile_Base.TileType.teleporter:
                teleporterList.RemoveTileFromList(tile);
                break;
            case Tile_Base.TileType.speedBoost:
                speedBoostList.RemoveTileFromList(tile);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Puts an object on a random free space on the grid.
    /// Does this by checking the neutral list, and changing a random tile.
    /// </summary>
    /// <param name="typeToPlace">Type of object I want on the grid.</param>
    public void PlaceObjectRandomlyOnGrid(Tile_Base.TileType typeToPlace)
    {
        
        if(neutralList.GetCount() == 0) { return; } //no free spaces
        if(tileGrid == null) { return; }

        int min = 0;
        int max = neutralList.GetCount();
        int randomInt = UnityEngine.Random.Range(min, max);

        Tile_Base newTile = GetTileFromList(randomInt, neutralList);

        tileGrid.ChangeTile((int)newTile.GetTileIndex().x, (int)newTile.GetTileIndex().y, typeToPlace);
    }

    public Tile_Base GetTileFromList( int index, TileList tileList)
    {
        //getTile checks for in bounds.
        return tileList.GetTile(index);
    }

}
