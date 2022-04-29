using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileList : MonoBehaviour
{
    private Tile.TileType tileType;
    private TileTracker tileTracker = null;

    public List<Tile> tileList = new List<Tile>();

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
    public void SetTileType(Tile.TileType newTileType)
    {
        tileType = newTileType;
    }

    /// <summary>
    /// Gets the current type of tile this list takes.
    /// </summary>
    /// <returns>Current tile type of the list.</returns>
    public Tile.TileType GetTileType()
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
    public Tile GetTile(int index)
    {
        if(index < 0 || index >= tileList.Count)
        {
            Tile newTile = null;
            return newTile;
        }
        return tileList[index];
    }

    /// <summary>
    /// Adds tiles to the list. Critters are added to the front.
    /// </summary>
    /// <param name="tile">Tile to add.</param>
    public void AddTileToList(Tile tile)
    {

        Tile.TileType type = tile.GetTileType();

        if(type != tileType) { return; }

        if(type == Tile.TileType.critter)
        {
            tileList.Insert(0, tile);
        }
        else
        {
            tileList.Add(tile);
        }
    }

    /// <summary>
    /// Takes tiles off of the list. Critters are removed from the back.
    /// </summary>
    /// <param name="tile">Tile to remove.</param>
    public void RemoveTileFromList(Tile tile)
    {
        Tile.TileType type = tile.GetTileType();

        if (type != tileType) { return; }

        if(type == Tile.TileType.critter)
        {
            tileList.RemoveAt(tileList.Count - 1);
        }
        else
        {
            tileList.Remove(tile);
        }
    }

    /// <summary>
    /// Changes the Tile type in a list. Then adds it to the correct list and
    /// removes it from the current list.
    /// </summary>
    /// <param name="index">Slot in list where we want to make change</param>
    /// <param name="newType">Type to change to</param>
    public void ChangeTileInList(int index, Tile.TileType newType)
    {
        if (index < 0 || index > tileList.Count) { return; }

        if(newType == tileType) { return; }    //not changing tiles because they are the same type

        tileList[index].SetTileType(newType);       //change to the new type
        tileTracker.AddTileToList(tileList[index]); //add to list
        tileList.Remove(tileList[index]);           //remove from this list
    }

}
