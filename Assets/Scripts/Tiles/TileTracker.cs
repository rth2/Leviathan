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

        neutralList.SetTileType(Tile.TileType.neutral);
        critterList.SetTileType(Tile.TileType.critter);
        foodList.SetTileType(Tile.TileType.food);
        wallList.SetTileType(Tile.TileType.wall);
        teleporterList.SetTileType(Tile.TileType.teleporter);
        speedBoostList.SetTileType(Tile.TileType.speedBoost);
        movingObstacleList.SetTileType(Tile.TileType.movingObstacle);
    }

    private void Start()
    {
        if (gameLoop == null) { return; }
        gameLoop.OnNewTickCycle += MoveCritter;
        gameLoop.OnNewTickCycle += MoveMoveableObstacles;

        settings = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<gameSettings>();
        if(settings == null) { return; }

        audioHandler = settings.GetComponent<AudioHandler>();
    }

    public TileList GetTileList(Tile.TileType listType)
    {
        TileList listToGet = null;

        switch(listType)
        {
            case Tile.TileType.critter:
                listToGet = critterList;
                break;
            case Tile.TileType.food:
                listToGet = foodList;
                break;
            case Tile.TileType.neutral:
                listToGet = neutralList;
                break;
            case Tile.TileType.wall:
                listToGet = wallList;
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

        for(int i =0; i < movingObstacleList.GetCount(); i++)
        {
            Tile obstacleTile = GetTileFromList(i, movingObstacleList);

            Vector2 obstacleTileIndex = obstacleTile.GetTileIndex();

            float moveToTileX = obstacleTile.GetMoveableObstacleDirection().x + obstacleTileIndex.x;
            float moveToTileY = obstacleTile.GetMoveableObstacleDirection().y + obstacleTileIndex.y;

            Tile moveToThisTile = tileGrid.GetTileFromTileGrid((int)moveToTileX, (int)moveToTileY);

            switch(moveToThisTile.GetTileType())
            {
                case Tile.TileType.neutral:
                    tileGrid.ChangeTileType((int)moveToThisTile.GetTileIndex().x, (int)moveToThisTile.GetTileIndex().y, Tile.TileType.movingObstacle);
                    break;
                case Tile.TileType.speedBoost:
                    tileGrid.ChangeTileType((int)moveToThisTile.GetTileIndex().x, (int)moveToThisTile.GetTileIndex().y, Tile.TileType.movingObstacle);
                    break;
                default:  //anything else and this should break
                    break;
            }

            if (obstacleTile.GetIsSpeedBoost())
                tileGrid.ChangeTileType((int)obstacleTileIndex.x, (int)obstacleTileIndex.y, Tile.TileType.speedBoost);
            else
                tileGrid.ChangeTileType((int)obstacleTileIndex.x, (int)obstacleTileIndex.y, Tile.TileType.neutral);
        }

    }

    public void MoveCritter()
    {
        if(tileGrid == null) { return; }
        if(critter == null) { return; }

        Tile critterHead = GetTileFromList(0, critterList);

        Vector2 critterHeadIndex = critterHead.GetTileIndex();
        critter.CalculateDirection();
        Vector2 directionToMove = critter.GetDirection();

        int newCritterHeadIndexX = Mathf.FloorToInt(critterHeadIndex.x + directionToMove.x);
        int newCritterHeadIndexY = Mathf.FloorToInt(critterHeadIndex.y + directionToMove.y);

        Tile newCritterHead = tileGrid.GetTileFromTileGrid(newCritterHeadIndexX, newCritterHeadIndexY);
        if (newCritterHead == null) { return; }

        switch(newCritterHead.GetTileType())
        {
            case Tile.TileType.neutral:
                {
                    MoveCritterTail();

                    tileGrid.ChangeTileType(newCritterHeadIndexX, newCritterHeadIndexY, Tile.TileType.critter);
                    break;
                }
            case Tile.TileType.food:
                {
                    tileGrid.ChangeTileType(newCritterHeadIndexX, newCritterHeadIndexY, Tile.TileType.critter);

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
            case Tile.TileType.teleporter:
                {
                    audioHandler.PlaySoundFX(AudioHandler.AUDIO_FX.teleporter, audioSource);
                    newCritterHead = newCritterHead.GetTeleporterPair();

                    MoveCritterTail();

                    tileGrid.ChangeTileType((int)newCritterHead.GetTileIndex().x, (int)newCritterHead.GetTileIndex().y, Tile.TileType.critter);
                    MoveCritter();
                    break;
                }
            case Tile.TileType.speedBoost:
                {
                    if(gameLoop == null) { return; }

                    audioHandler.PlaySoundFX(AudioHandler.AUDIO_FX.speedBoost, audioSource);

                    MoveCritterTail();

                    tileGrid.ChangeTileType(newCritterHeadIndexX, newCritterHeadIndexY, Tile.TileType.critter);

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

    private void MoveCritterTail()
    {
        Tile oldCritterTail = GetTileFromList(critterList.GetCount() - 1, critterList);
        if (oldCritterTail == null) { return; }
        Vector2 critterTailIndex = oldCritterTail.GetTileIndex();

        int critterTailIndexX = Mathf.FloorToInt(critterTailIndex.x);
        int critterTailIndexY = Mathf.FloorToInt(critterTailIndex.y);

        if (oldCritterTail.GetIsSpeedBoost())
            tileGrid.ChangeTileType(critterTailIndexX, critterTailIndexY, Tile.TileType.speedBoost);
        else if (oldCritterTail.GetIsTeleporter())
            tileGrid.ChangeTileType(critterTailIndexX, critterTailIndexY, Tile.TileType.teleporter);
        else
            tileGrid.ChangeTileType(critterTailIndexX, critterTailIndexY, Tile.TileType.neutral);
    }

    public void AddTileToList( Tile tile)
    {
        switch (tile.GetTileType()){
            case Tile.TileType.neutral:
                neutralList.AddTileToList(tile);
                break;
            case Tile.TileType.critter:
                critterList.AddTileToList(tile);
                break;
            case Tile.TileType.food:
                foodList.AddTileToList(tile);
                break;
            case Tile.TileType.wall:
                wallList.AddTileToList(tile);
                break;
            case Tile.TileType.movingObstacle:
                movingObstacleList.AddTileToList(tile);
                if(critter == null) { return; }
                tile.SetMoveableObstacleDirection(Vector2.right);
                break;
            case Tile.TileType.teleporter:
                teleporterList.AddTileToList(tile);
                break;
            case Tile.TileType.speedBoost:
                tile.SetIsSpeedBoost(true);
                speedBoostList.AddTileToList(tile);
                break;
            default:
                break;
        }
    }

    public void RemoveTileFromList(Tile tile)
    {
        switch (tile.GetTileType())
        {
            case Tile.TileType.neutral:
                neutralList.RemoveTileFromList(tile);
                break;
            case Tile.TileType.critter:
                critterList.RemoveTileFromList(tile);
                break;
            case Tile.TileType.food:
                foodList.RemoveTileFromList(tile);
                if (neutralList.GetCount() == 0 && foodList.GetCount() == 0)
                {
                    sceneHandler.HandleVictory();
                }
                break;
            case Tile.TileType.wall:
                wallList.RemoveTileFromList(tile);
                break;
            case Tile.TileType.movingObstacle:
                movingObstacleList.RemoveTileFromList(tile);
                break;
            case Tile.TileType.teleporter:
                teleporterList.RemoveTileFromList(tile);
                break;
            case Tile.TileType.speedBoost:
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
    public void PlaceObjectRandomlyOnGrid(Tile.TileType typeToPlace)
    {
        if(neutralList.GetCount() == 0) { return; } //no free spaces

        int min = 0;
        int max = neutralList.GetCount();
        int randomInt = UnityEngine.Random.Range(min, max);

        Tile newTile = GetTileFromList(randomInt, neutralList);

        neutralList.ChangeTileInList(randomInt, typeToPlace);
    }

    public Tile GetTileFromList( int index, TileList tileList)
    {
        //getTile checks for in bounds.
        return tileList.GetTile(index);
    }

}
