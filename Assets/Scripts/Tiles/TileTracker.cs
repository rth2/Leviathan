using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTracker : MonoBehaviour
{
    [SerializeField]TileList neutral, snake, food, wall;

    private void Awake()
    {
        neutral = new TileList();
        snake = new TileList();
        food = new TileList();
        wall = new TileList();

        neutral.SetTileTracker();
        snake.SetTileTracker();
        food.SetTileTracker();
        wall.SetTileTracker();

        neutral.SetTileType(Tile.TileType.neutral);
        snake.SetTileType(Tile.TileType.snake);
        food.SetTileType(Tile.TileType.food);
        wall.SetTileType(Tile.TileType.wall);

    }

    public void PrintCounts()
    {
        Debug.Log($"Size of neutral :{neutral.GetCount()}");
        Debug.Log($"Size of snake :{snake.GetCount()}");
        Debug.Log($"Size of food :{food.GetCount()}");
        Debug.Log($"Size of wall :{wall.GetCount()}");
    }

    public void AddTileToList( Tile tile)
    {
        switch (tile.GetTileType()){
            case Tile.TileType.neutral:
                neutral.AddTileToList(tile);
                break;
            case Tile.TileType.snake:
                snake.AddTileToList(tile);
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
            case Tile.TileType.snake:
                snake.RemoveTileFromList(tile);
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
