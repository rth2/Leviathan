using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicCreateFood : MonoBehaviour
{
    Tile.TileType food = Tile.TileType.food;

    [SerializeField] TileGrid grid = null;

    private void Update()
    {
        PlaceFood();
        
    }

    private void PlaceFood()
    {
        if (grid == null) return;

        int randomRow = Random.Range(1, grid.GetMaxRows() + 1);
        int randomCol = Random.Range(1, grid.GetMaxCols() + 1);

        grid.ChangeTileType(randomRow, randomCol, food);

    }

}
