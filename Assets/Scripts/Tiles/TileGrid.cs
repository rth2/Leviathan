using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class TileGrid : MonoBehaviour
{
    #region Attributes

    [Header("Types of Tiles")]
    [SerializeField] GameObject critterTile = null;
    [SerializeField] GameObject foodTile = null;
    [SerializeField] GameObject movingObstacleTile = null;
    [SerializeField] GameObject neutralTile = null;
    [SerializeField] GameObject speedBoostTile = null;
    [SerializeField] GameObject teleporterTile = null;
    [SerializeField] GameObject wallTile = null;


    [Header("Tile Grid Attributes")]
    [SerializeField] GameObject tile = null;
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

    gameSettings settings = null;

    #endregion

    private void Awake()
    {
        settings = gameSettings.Instance;
    }

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

        if(settings == null) { return; }

        leftBoundary.transform.position = new Vector3(-Mathf.Ceil(((float)(settings.GetNumberOfRows() + boundaryTiles)) * 0.5f), 0, 0);
        rightBoundary.transform.position = new Vector3(Mathf.Ceil(((float)(settings.GetNumberOfRows() + boundaryTiles)) * 0.5f), 0, 0);
        topBoundary.transform.position = new Vector3(0, Mathf.Ceil(((float)(settings.GetNumberOfCols() + boundaryTiles)) * 0.5f), 0);
        botBoundary.transform.position = new Vector3(0, -Mathf.Ceil(((float)(settings.GetNumberOfCols() + boundaryTiles)) * 0.5f), 0);
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
        if(settings == null) { return; }

        int startingRow = Mathf.FloorToInt(settings.GetNumberOfRows() * 0.5f);
        int startingCol = Mathf.FloorToInt(settings.GetNumberOfCols() * 0.5f);
        int critterLength = critter.GetLength();

        for (int i = 0; i < critterLength; i++)
        {
            ChangeTile(startingRow - i, startingCol, Tile_Base.TileType.critter);
        }
    }

    /// <summary>
    /// Checks to see if the tile is in a valid index on the Grid. If yes, it returns the tile.
    /// Otherwise it returns a null tile.
    /// </summary>
    /// <param name="row">Row of the tile you want. X coord.</param>
    /// <param name="col">Col of the tile you want. Y coord.</param>
    /// <returns>The requested Tile. Null if out of bounds.</returns>
    public Tile_Base GetTileFromTileGrid(int row, int col)
    {
        Tile_Base tile = null;

        if (tileGrid == null) {  return tile; }
        if(settings == null) { return tile; }

        if (row < 0 || row >= settings.GetNumberOfRows() + boundaryTiles) {  return tile; }
        if (col < 0 || col >= settings.GetNumberOfCols() + boundaryTiles) {  return tile; }

        tile = tileGrid[row, col].GetComponent<Tile_Base>();
        return tile;
    }

    /// <summary>
    /// Builds a grid of tiles from the top left to the bottom right.
    /// Fields for size and tile are set in the inspector.
    /// Takes a tile that is square and 1 unity unit in size.
    /// </summary>
    private void BuildTileGrid()
    {
        if (settings == null) { return; }

        int rowAmount = settings.GetNumberOfRows() + boundaryTiles;
        int colAmount = settings.GetNumberOfCols() + boundaryTiles;
        Vector3 tileToPlacePos = new Vector3();
        float rowAmountX, colAmountY;

        tileGrid = new GameObject[rowAmount, colAmount];

        if (rowAmount % 2 == 0)
            rowAmountX = -((float)rowAmount * 0.5f);
        else
            rowAmountX = -((float)rowAmount * 0.5f) + tileOffset;

        if (colAmount % 2 == 0)
            colAmountY = ((float)colAmount * 0.5f);
        else
            colAmountY = ((float)colAmount * 0.5f) - tileOffset;

        for (int i = 0; i < rowAmount; i++)
        {
            for (int j = 0; j < colAmount; j++)
            {
                tileToPlacePos.x = rowAmountX + i;
                tileToPlacePos.y = colAmountY - j;

                //border tiles / walls
                if (i == 0 || i == rowAmount - 1 || j == 0 || j == colAmount - 1)
                {
                    tileGrid[i, j] = Instantiate(wallTile, tileToPlacePos, Quaternion.identity, tileCache.transform);
                }
                else
                {   //every other tile starts neutral
                    tileGrid[i, j] = Instantiate(neutralTile, tileToPlacePos, Quaternion.identity, tileCache.transform);

                    if (tileTracker != null)
                    {
                        tileTracker.AddTileToList(tileGrid[i,j].GetComponent<Tile_Base>());
                    }
                }

                Tile_Base tileBase = tileGrid[i, j].GetComponent<Tile_Base>();

                tileBase.SetTilePosition(Mathf.FloorToInt(tileToPlacePos.x), Mathf.FloorToInt(tileToPlacePos.y));
                tileBase.SetTileIndex(i, j);
            }
        }
    }

    /// <summary>
    /// Change the type of a Tile on the grid in [row, col].
    /// </summary>
    /// <param name="row">Row to check if we can change.</param>
    /// <param name="col">Column to check if we can change.</param>
    /// <param name="type">Request a change to this type.</param>
    public void ChangeTile(int row, int col, Tile_Base.TileType type)
    {
        if(tileGrid == null) { return; }

        Tile_Base tileToChange = GetTileFromTileGrid(row, col); //tile that needs to change

        if(tileToChange == null) { return; }
        if(tileToChange.GetTileType() == type) { return; }
        if(tileTracker == null) { return; }

        Vector2 oldTilePosition = tileToChange.GetTilePosition();

        //Remember Speedboost and teleporters
        if (tileToChange.GetTileType() != Tile_Base.TileType.speedBoost && tileToChange.GetTileType() != Tile_Base.TileType.teleporter)
        {
            tileTracker.RemoveTileFromList(tileToChange);
            Destroy(tileToChange.gameObject);
        }
        else
        {   //turn off teleporter or speed boost object.
            tileToChange.gameObject.SetActive(false);
        }
            
        GameObject tileToAdd = null; //need a tile for this location on the grid.

        if(type == Tile_Base.TileType.speedBoost)
        {
            if(tileTracker.GetTileList(Tile_Base.TileType.speedBoost).GetCount() > 0)
            {
                Tile_Base tile = tileTracker.GetTileList(Tile_Base.TileType.speedBoost).GetTileByRowCol(row, col);

                if (tile != null)
                    tileToAdd = tileTracker.GetTileList(Tile_Base.TileType.speedBoost).GetTileByRowCol(row, col).gameObject;
            }
        }
        else if(type == Tile_Base.TileType.teleporter)
        {
            if (tileTracker.GetTileList(Tile_Base.TileType.teleporter).GetCount() > 0)
            {
                Tile_Base tile = tileTracker.GetTileList(Tile_Base.TileType.teleporter).GetTileByRowCol(row, col);

                if (tile != null)
                    tileToAdd = tileTracker.GetTileList(Tile_Base.TileType.teleporter).GetTileByRowCol(row, col).gameObject;
            }
        }

        //did not find a matching tile in a list.
        if (tileToAdd == null)
        {
            tileToAdd = CreateTileOfType(type);

            Tile_Base tileBase = tileToAdd.GetComponent<Tile_Base>();
            tileBase.SetTileIndex(row, col);
            tileBase.SetTilePosition((int)oldTilePosition.x, (int)oldTilePosition.y);
            tileToAdd.transform.parent = tileCache.transform;
            tileToAdd.transform.position = oldTilePosition;
            tileTracker.AddTileToList(tileBase);
        }
        else
            tileToAdd.gameObject.SetActive(true);

        tileGrid[row, col] = tileToAdd;
    }


    private GameObject CreateTileOfType(Tile_Base.TileType type)
    {
        GameObject newTile;

        switch(type)
        {
            case Tile_Base.TileType.critter:
                newTile = Instantiate(critterTile);
                break;
            case Tile_Base.TileType.food:
                newTile = Instantiate(foodTile);
                break;
            case Tile_Base.TileType.movingObstacle:
                newTile = Instantiate(movingObstacleTile);
                break;
            case Tile_Base.TileType.neutral:
                newTile = Instantiate(neutralTile);
                break;
            case Tile_Base.TileType.speedBoost:
                newTile = Instantiate(speedBoostTile);
                break;
            case Tile_Base.TileType.teleporter:
                newTile = Instantiate(teleporterTile);
                break;
            case Tile_Base.TileType.wall:
                newTile = Instantiate(wallTile);
                break;
            default:
                newTile = Instantiate(neutralTile);
                break;
        }
        newTile.transform.parent = tileCache.transform;
        return newTile;
    }

}
