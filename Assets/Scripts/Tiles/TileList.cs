using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileList : MonoBehaviour
{
    private Tile_Base.TileType tileType;
    private TileTracker tileTracker = null;

    public List<Tile_Base> tileList = new List<Tile_Base>();

    /// <summary>
    /// Had to set the tile tracker manually as lists are made.
    /// </summary>
    public void SetTileTracker()
    {
        tileTracker = GameObject.FindGameObjectWithTag("TileTracker").GetComponent<TileTracker>();
    }

    /// <summary>
    /// Sets what type of tiles list will accept.
    /// </summary>
    /// <param name="newTileType">Tile type to take; gotten from TileType enum.</param>
    public void SetTileType(Tile_Base.TileType newTileType)
    {
        tileType = newTileType;
    }

    /// <summary>
    /// Gets the current type of tile this list takes.
    /// </summary>
    /// <returns>Current tile type of the list.</returns>
    public Tile_Base.TileType GetTileType()
    {
        return tileType;
    }

    /// <summary>
    /// Gets how many elements are in the list.
    /// </summary>
    /// <returns>Number of elements in list.</returns>
    public int GetCount()
    {
        return tileList.Count;
    }

    /// <summary>
    /// Gets the tile at the requested index. Returns null if out of bounds.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>Tile at index or null.</returns>
    public Tile_Base GetTile(int index)
    {
        if(index < 0 || index >= tileList.Count)
        {
            Tile_Base newTile = null;
            return newTile;
        }
        return tileList[index];
    }

    public Tile_Base GetTileByRowCol(int row, int col)
    {
        for(int i = 0; i < tileList.Count; i++)
        {   //found tile matching row and col
            if ((int)tileList[i].GetTileIndex().x == row && (int)tileList[i].GetTileIndex().y == col)
                return tileList[i];
                
        }
        return null;
    }

    /// <summary>
    /// Adds tiles to the list. Critters are added to the front.
    /// </summary>
    /// <param name="tile">Tile to add.</param>
    public void AddTileToList(Tile_Base tile)
    {

        Tile_Base.TileType type = tile.GetTileType();

        if(type != tileType) { return; }

        if(type == Tile_Base.TileType.critter || type == Tile_Base.TileType.teleporter)
            tileList.Insert(0, tile);
        else
            tileList.Add(tile);

        if(type == Tile_Base.TileType.teleporter)
        {
            Tile_Teleporter teleporterTile = tile.GetComponent<Tile_Teleporter>();

            if (teleporterTile.GetTeleporterPair() != null)
            {
                tileList[0].SetColor(teleporterTile.GetTeleporterPair().GetColor());
                return;
            }

            if(tileList.Count % 2 == 0)
            {
                teleporterTile.SetTeleporterPair(tileList[1].GetComponent<Tile_Teleporter>());
                tileList[1].GetComponent<Tile_Teleporter>().SetTeleporterPair(tileList[0].GetComponent<Tile_Teleporter>());

                Color color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
                tileList[0].SetColor(color);
                tileList[1].SetColor(color);

            }
        }
    }

    /// <summary>
    /// Takes tiles off of the list. Critters are removed from the back.
    /// </summary>
    /// <param name="tile">Tile to remove.</param>
    public void RemoveTileFromList(Tile_Base tile)
    {
        Tile_Base.TileType type = tile.GetTileType();

        if (type != tileType) { return; }

        if(type == Tile_Base.TileType.critter)
            tileList.RemoveAt(tileList.Count - 1);
        else
            tileList.Remove(tile);
    }

}
