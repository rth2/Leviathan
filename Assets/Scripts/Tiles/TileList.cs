using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileList : MonoBehaviour
{
    private Tile.TileType tileType;
    private TileTracker tileTracker = null;

    [SerializeField] public List<Tile> tileList = new List<Tile>();

    public void SetTileTracker()
    {
        tileTracker = GameObject.FindGameObjectWithTag("TileTracker").GetComponent<TileTracker>();
    }

    public void SetTileType(Tile.TileType newTileType)
    {
        tileType = newTileType;
    }

    public Tile.TileType GetTileType()
    {
        return tileType;
    }

    public int GetCount()
    {
        return tileList.Count;
    }

    public Tile GetTile(int index)
    {
        return tileList[index];
    }

    public void AddTileToList(Tile tile)
    {
        if(tile.GetTileType() != tileType) { return; }

        tileList.Add(tile);
    }

    public void RemoveTileFromList(Tile tile)
    {
        if (tile.GetTileType() != tileType) { return; }

        tileList.Remove(tile);
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
