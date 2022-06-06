using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_MovingObstacle : Tile_Base
{
    Vector2 moveObstacleDirection = new Vector2();

    protected override void Start()
    {
        SetMoveableObstacleDirection(Vector2.right);
    }

    public override Vector2 GetMoveableObstacleDirection()
    {
        return moveObstacleDirection;
    }

    public void SetMoveableObstacleDirection(Vector2 direction)
    {
        moveObstacleDirection = direction;
    }



}
