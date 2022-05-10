using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTracker : MonoBehaviour
{
    [SerializeField] TileGrid tileGrid = null;

    [Header("Objects Connected")]
    [SerializeField] GameLoop gameLoop = null;
    [SerializeField] Critter critter = null;
    [SerializeField] SceneHandler sceneHandler = null;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource = null;

    gameSettings settings = null;
    AudioHandler audioHandler = null;
    TileList neutralList, critterList, foodList, wallList;

    /// <summary>
    /// Sets up the different lists of tiles.
    /// </summary>
    private void Awake()
    {
        neutralList = new TileList();
        critterList = new TileList();
        foodList = new TileList();
        wallList = new TileList();

        neutralList.SetTileTracker();
        critterList.SetTileTracker();
        foodList.SetTileTracker();
        wallList.SetTileTracker();

        neutralList.SetTileType(Tile.TileType.neutral);
        critterList.SetTileType(Tile.TileType.critter);
        foodList.SetTileType(Tile.TileType.food);
        wallList.SetTileType(Tile.TileType.wall);
    }

    private void Start()
    {
        if (gameLoop == null) { return; }
        gameLoop.OnNewTickCycle += MoveCritter;

        settings = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<gameSettings>();
        if(settings == null) { return; }

        audioHandler = settings.GetComponent<AudioHandler>();
    }

    public void MoveCritter()
    {
        if(tileGrid == null) { return; }
        if(critter == null) { return; }

        Tile critterHead = GetTileFromList(0, critterList);

        Vector2 critterHeadIndex = critterHead.GetTileIndex();
        Vector2 directionToMove = critter.GetDirection();

        int newCritterHeadIndexX = Mathf.FloorToInt(critterHeadIndex.x + directionToMove.x);
        int newCritterHeadIndexY = Mathf.FloorToInt(critterHeadIndex.y + directionToMove.y);

        Tile newCritterHead = tileGrid.GetTileFromTileGrid(newCritterHeadIndexX, newCritterHeadIndexY);
        if (newCritterHead == null) { return; }

        if (newCritterHead.GetTileType() == Tile.TileType.neutral)
        {
            Tile oldCritterTail = GetTileFromList(critterList.GetCount() - 1, critterList);
            if (oldCritterTail == null) { return; }
            Vector2 critterTailIndex = oldCritterTail.GetTileIndex();

            int critterTailIndexX = Mathf.FloorToInt(critterTailIndex.x);
            int critterTailIndexY = Mathf.FloorToInt(critterTailIndex.y);

            tileGrid.ChangeTileType(critterTailIndexX, critterTailIndexY, Tile.TileType.neutral);
            tileGrid.ChangeTileType(newCritterHeadIndexX, newCritterHeadIndexY, Tile.TileType.critter);
        }
        else if(newCritterHead.GetTileType() == Tile.TileType.food)
        {
            tileGrid.ChangeTileType(newCritterHeadIndexX, newCritterHeadIndexY, Tile.TileType.critter);
            if(audioHandler)
            {
                audioHandler.PlaySoundFX(AudioHandler.AUDIO_FX.eatFood, audioSource);
            }

            critter.AddLength(1);
            if(foodList.GetCount() == 0)
            {
                tileGrid.PlaceFoodOnGrid();
            }
            
        }   
        else  //all that are left are walls and snakes
        {
            if (audioHandler)
            {
                audioHandler.PlaySoundFX(AudioHandler.AUDIO_FX.critterDie, audioSource);
            }
            sceneHandler.HandleDeath();
        }
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
