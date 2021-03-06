using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicCreateFood : MonoBehaviour
{

    [SerializeField] TileTracker tileTracker = null;
    [SerializeField] float increaseSpeedAmount = 0.1f;


    Tile_Base.TileType food = Tile_Base.TileType.food;

    public void PlaceFood()
    {
        if(tileTracker == null) { return; }

        tileTracker.PlaceObjectRandomlyOnGrid(food);
    }

    public float GetIncreaseSpeedAmount()
    {
        return increaseSpeedAmount;
    }


}
