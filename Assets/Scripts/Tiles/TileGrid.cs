using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class TileGrid : MonoBehaviour
{

    [Header("Tile Grid Attributes")]
    [SerializeField] GameObject tile = null;
    [SerializeField] int maxRows = 25, maxCols = 19;
    [SerializeField] GameObject tileCache = null;
    [SerializeField] TileTracker tileTracker = null;

    GameObject leftBoundary, rightBoundary, topBoundary, botBoundary;

    [Header("Objects on Grid")]
    [SerializeField] Critter critter = null;
    [SerializeField] ClassicCreateFood obstacleCreator = null;

    [Header("Camera for Grid Bounds")]
    [SerializeField] Cinemachine.CinemachineTargetGroup cinemachineTargetGroup = null;

    GameObject[,] tileGrid;     //accessor for tiles

    float tileOffset = 0.5f;    //tiles should be 1 unity unit. This offsets the tile to match the unity grid on whole integers.
    int boundaryTiles = 2;      //boundary wall for the tileGrid. One on each side of X; one on each side of Y.


    private void Start()
    {
        BuildBoundaryObjects();
        BuildTileGrid();
        SetGameCameraBoundaries();
        PlaceCritterOnGrid();
        PlaceFoodOnGrid();
    }

    /// <summary>
    /// Creates, names, and places the boundary objects to be used in the Cinemachine target group.
    /// </summary>
    private void BuildBoundaryObjects()
    {
        leftBoundary = new GameObject();
        leftBoundary.name = "leftBoundary";
        rightBoundary = new GameObject();
        rightBoundary.name = "rightBoundary";
        topBoundary = new GameObject();
        topBoundary.name = "topBoundary";
        botBoundary = new GameObject();
        botBoundary.name = "botBoundary";

        leftBoundary.transform.position = new Vector3(-Mathf.Ceil(((float)(maxRows + boundaryTiles)) * 0.5f), 0, 0);
        rightBoundary.transform.position = new Vector3(Mathf.Ceil(((float)(maxRows + boundaryTiles)) * 0.5f), 0, 0);
        topBoundary.transform.position = new Vector3(0, Mathf.Ceil(((float)(maxCols + boundaryTiles)) * 0.5f), 0);
        botBoundary.transform.position = new Vector3(0, -Mathf.Ceil(((float)(maxCols + boundaryTiles)) * 0.5f), 0);
    }
    /// <summary>
    /// Changes cinemachine target group based on our 4 boundaries.
    /// Only have 4 targets available in the inspector.
    /// </summary>
    private void SetGameCameraBoundaries()
    {
        //cinemachineTargetGroup.AddMember(leftBoundary.transform, 1, 0);
        //cinemachineTargetGroup.AddMember(rightBoundary.transform, 1, 0);
        //cinemachineTargetGroup.AddMember(topBoundary.transform, 1, 0);
        //cinemachineTargetGroup.AddMember(botBoundary.transform, 1, 0);

        cinemachineTargetGroup.m_Targets[0].target = leftBoundary.transform;
        cinemachineTargetGroup.m_Targets[1].target = rightBoundary.transform;
        cinemachineTargetGroup.m_Targets[2].target = topBoundary.transform;
        cinemachineTargetGroup.m_Targets[3].target = botBoundary.transform;
    }

    /// <summary>
    /// Places the first food on the Grid.
    /// </summary>
    public void PlaceFoodOnGrid()
    {
        obstacleCreator.PlaceFood();
    }

    /// <summary>
    /// Places critter(player) on grid center at beginning of game.
    /// Builds to right if critter is longer than 1.
    /// Critter Head will be on the left to match starting direction.
    /// </summary>
    private void PlaceCritterOnGrid()
    {
        if(critter == null) { return; }

        int startingRow = Mathf.FloorToInt(maxRows * 0.5f);
        int startingCol = Mathf.FloorToInt(maxCols * 0.5f);
        int critterLength = critter.GetLength();

        for (int i = 0; i < critterLength; i++)
        {
            ChangeTileType(startingRow - i, startingCol, Tile.TileType.critter);
        }
    }

    /// <summary>
    /// Checks to see if the tile is in a valid index on the Grid. If yes, it returns the tile.
    /// Otherwise it returns a null tile.
    /// </summary>
    /// <param name="row">Row of the tile you want. X coord.</param>
    /// <param name="col">Col of the tile you want. Y coord.</param>
    /// <returns>The requested Tile. Null if out of bounds.</returns>
    public Tile GetTileFromTileGrid(int row, int col)
    {
        Tile tile = null;
        
        //having issue where grid is 0,0 at corner but pos is 0,0 at center.
        //need to set my actual row/col when tiles are first made and store those.

        if (tileGrid == null) {  return tile; }
        if (row < 0 || row >= maxRows + boundaryTiles) {  return tile; }
        if (col < 0 || col >= maxCols + boundaryTiles) {  return tile; }

        tile = tileGrid[row, col].GetComponent<Tile>();
        return tile;
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

                    componentTile.SetTilePosition(Mathf.FloorToInt(tileToPlacePos.x), Mathf.FloorToInt(tileToPlacePos.y));
                    componentTile.SetTileIndex(i,j);
                }
            }
        }
    }

    /// <summary>
    /// Change the type of a Tile on the grid in [row, col].
    /// </summary>
    /// <param name="row">Row to check if we can change.</param>
    /// <param name="col">Column to check if we can change.</param>
    /// <param name="type">Request a change to this type.</param>
    public void ChangeTileType(int row, int col, Tile.TileType type)
    {
        if(tileGrid == null) { return; }

        Tile tileToChange = GetTileFromTileGrid(row, col);

        if(tileToChange == null) { return; }
        if(tileToChange.GetTileType() == type) { return; }
        if(tileTracker == null) { return; }

        tileTracker.RemoveTileFromList(tileToChange);
        tileToChange.SetTileType(type);
        tileTracker.AddTileToList(tileToChange);
    }

}
