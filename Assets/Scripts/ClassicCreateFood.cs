using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicCreateFood : MonoBehaviour
{
    Tile.TileType food = Tile.TileType.food;

    [SerializeField] TileTracker tileTracker = null;

    public void PlaceFood()
    {
        if(tileTracker == null) { return; }

        tileTracker.PlaceObjectRandomlyOnGrid(food);
    }


}
