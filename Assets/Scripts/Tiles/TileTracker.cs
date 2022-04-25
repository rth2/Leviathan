using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTracker : MonoBehaviour
{
    public List<Tile> tileList = new List<Tile>();



    public void AddTileToList(Tile tile)
    {
        tileList.Add(tile);
    }

    public int GetCount()
    {
        return tileList.Count;
    }

    public void RemoveTileFromList(Tile tile)
    {
        tileList.Remove(tile);
    }

    public void ChangeTileInList(int index, Tile.TileType type)
    {
        if(index < 0 || index > tileList.Count) { return; }

        tileList[index].SetTileType(type);
        if(type != Tile.TileType.neutral)
        {
            tileList.Remove(tileList[index]);
        }
    }

}
