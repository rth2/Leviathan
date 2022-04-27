using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTracker : MonoBehaviour
{
    [SerializeField]TileList neutral, critter, food, wall;

    private void Awake()
    {
        neutral = new TileList();
        critter = new TileList();
        food = new TileList();
        wall = new TileList();

        neutral.SetTileTracker();
        critter.SetTileTracker();
        food.SetTileTracker();
        wall.SetTileTracker();

        neutral.SetTileType(Tile.TileType.neutral);
        critter.SetTileType(Tile.TileType.critter);
        food.SetTileType(Tile.TileType.food);
        wall.SetTileType(Tile.TileType.wall);
    }

    public void AddTileToList( Tile tile)
    {
        switch (tile.GetTileType()){
            case Tile.TileType.neutral:
                neutral.AddTileToList(tile);
                break;
            case Tile.TileType.critter:
                critter.AddTileToList(tile);
                break;
            case Tile.TileType.food:
                food.AddTileToList(tile);
                break;
            case Tile.TileType.wall:
                wall.AddTileToList(tile);
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
                neutral.RemoveTileFromList(tile);
                break;
            case Tile.TileType.critter:
                critter.RemoveTileFromList(tile);
                break;
            case Tile.TileType.food:
                food.RemoveTileFromList(tile);
                break;
            case Tile.TileType.wall:
                wall.RemoveTileFromList(tile);
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
        if(neutral.GetCount() == 0) { return; } //no free spaces

        int min = 0;
        int max = neutral.GetCount();
        int randomInt = UnityEngine.Random.Range(min, max);

        Tile newTile = GetTileFromList(randomInt, neutral);

        neutral.ChangeTileInList(randomInt, typeToPlace);
    }

    public Tile GetTileFromList( int index, TileList tileList)
    {
        return tileList.GetTile(index);
    }
}
