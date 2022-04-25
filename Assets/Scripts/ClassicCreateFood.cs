using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicCreateFood : MonoBehaviour
{
    Tile.TileType food = Tile.TileType.food;

    [SerializeField] TileGrid grid = null;
    [SerializeField] TileTracker tileTracker = null;


    public void PlaceFood()
    {
        if(tileTracker == null) { return; }
        if(tileTracker.GetCount() == 0) { return; }

        int min = 0;
        int max = tileTracker.GetCount();

        int randomInt = Random.Range(min, max);

        tileTracker.ChangeTileInList(randomInt, food);
        
    }


}
