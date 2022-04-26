using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    [SerializeField] int maxRows = 25, maxCols = 19;
    [SerializeField] GameObject tile = null;
    [SerializeField] GameObject tileCache = null;
    [SerializeField] TileTracker tileTracker = null;
    [SerializeField] Critter critter = null;
    [SerializeField] ClassicCreateFood foodCreator = null;

    GameObject[,] tileGrid; //accessor for tiles

    float tileOffset = 0.5f;
    int boundaryTiles = 2;  //boundary wall for the tileGrid. One on each side of X; one on each side of Y.


    private void Start()
    {
        BuildTileGrid();
        PlaceCritterOnGrid();
        PlaceInitialFood();
    }

    private void PlaceInitialFood()
    {
        foodCreator.PlaceFood();
    }

    /// <summary>
    /// Places the chosen critter on the tileGrid at the beginning of the game.
    /// </summary>
    private void PlaceCritterOnGrid()
    {
        if(critter == null) { return; }

        int startingRow = Mathf.FloorToInt(maxRows * 0.5f);
        int startingCol = Mathf.FloorToInt(maxCols * 0.5f);

        for (int i = 0; i < critter.GetLength(); i++)
        {
            ChangeTileType(startingRow + i, startingCol, Tile.TileType.snake);
        }
    }

    public int GetMaxRows()
    {
        return maxRows;
    }

    public int GetMaxCols()
    {
        return maxCols;
    }

    /// <summary>
    /// Builds a grid of tiles from the top left to the bottom right.
    /// Fields for size and tile are set in the inspector.
    /// Takes a tile that is square and 1 unity unit in size.
    /// </summary>
    private void BuildTileGrid()
    {
        int rowAmount = maxRows + boundaryTiles;
        int colAmount = maxCols + boundaryTiles;
        Vector3 tileToPlacePos = new Vector3();

        tileGrid = new GameObject[rowAmount, colAmount];

        for (int i = 0; i < rowAmount; i++)
        {
            for (int j = 0; j < colAmount; j++)
            {
                tileToPlacePos.x = -((float)rowAmount * 0.5f) + tileOffset + i;
                tileToPlacePos.y = ((float)colAmount * 0.5f) - tileOffset - j;

                tileGrid[i,j] = Instantiate(tile, tileToPlacePos, Quaternion.identity, tileCache.transform);

                if (tileGrid[i,j].TryGetComponent<Tile>(out Tile componentTile))
                {   //outmost tiles are border tiles / walls
                    if (i == 0 || i == rowAmount - 1 || j == 0 || j == colAmount - 1)
                    {
                        componentTile.SetTileType(Tile.TileType.wall);
                        tileTracker.AddTileToList(componentTile);
                    }
                    else
                    {   //every other tile starts neutral
                        componentTile.SetTileType(Tile.TileType.neutral);
                        if(tileTracker != null)
                        {
                            tileTracker.AddTileToList(componentTile);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="row">Row to check if we can change.</param>
    /// <param name="col">Column to check if we can change.</param>
    /// <param name="type">Request a change to this type.</param>
    public void ChangeTileType(int row, int col, Tile.TileType type)
    {
        if(tileGrid == null) { return; }

        if(row <= 0 || row >= maxRows + boundaryTiles) { return; }
        if(col <= 0 || col >= maxCols + boundaryTiles) { return; }

        if (tileGrid[row, col].TryGetComponent<Tile>(out Tile componentTile))
        {
            componentTile.SetTileType(type);

            if (type == Tile.TileType.neutral)
            {
                tileTracker.AddTileToList(componentTile);
            }
            else
            {
                tileTracker.RemoveTileFromList(componentTile);
            }
        }
    }

}
