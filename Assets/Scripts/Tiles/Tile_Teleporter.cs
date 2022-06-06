using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Teleporter : Tile_Base
{

    Tile_Teleporter teleporterPair = null;


    public void SetTeleporterPair(Tile_Teleporter pairToThisTile)
    {
        teleporterPair = pairToThisTile;
    }

    public override Tile_Base GetTeleporterPair()
    {
        return teleporterPair;
    }

}
