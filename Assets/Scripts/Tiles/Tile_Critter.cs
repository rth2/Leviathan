using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Critter : Tile_Base
{

    [SerializeField] Sprite[] headSpriteArray = new Sprite[4];      //0 is north, 1 is east, 2 is south, 3 is west
    [SerializeField] Sprite[] body01SpriteArray = new Sprite[4];    //0 is north, 1 is east, 2 is south, 3 is west
    [SerializeField] Sprite[] body02SpriteArray = new Sprite[4];    //0 is north, 1 is east, 2 is south, 3 is west
    [SerializeField] Sprite[] bodyBendsArray = new Sprite[4];       //0 is rightUp, 1 is rightDown, 2 is leftUp, 3 is leftDown

    Critter critter = null;

    public enum Direction
    {
        north = 0,
        east = 1,
        south = 2,
        west = 3
    };

    [SerializeField] public Direction direction;


    protected override void Start()
    {
        critter = GameObject.FindGameObjectWithTag("Player").GetComponent<Critter>();

        if(critter == null) { return; }

        SetSpriteDirection();
        SetSpriteHead();
    }

    private void SetSpriteDirection()
    {
        Vector2 critterDirection = critter.GetDirection();

        if (critterDirection.x == 1)
            direction = Direction.east;
        else if (critterDirection.x == -1)
            direction = Direction.west;
        else if (critterDirection.y == 1)
            direction = Direction.south;
        else
            direction = Direction.north;
    }

    public void SetBodySprite(bool body01)
    {
        if (body01)
            sr.sprite = body01SpriteArray[(int)direction];
        else
            sr.sprite = body02SpriteArray[(int)direction];

        SetBend();
    }

    public void SetSpriteHead()
    {
        sr.sprite = headSpriteArray[(int)direction];
    }

    private void SetBend()
    {
        Direction newHeadDirection;
        Vector2 critterDirection = critter.GetDirection();

        if (critterDirection.x == 1)
            newHeadDirection = Direction.east;
        else if (critterDirection.x == -1)
            newHeadDirection = Direction.west;
        else if (critterDirection.y == 1)
            newHeadDirection = Direction.south;
        else
            newHeadDirection = Direction.north;

        //heading the same direction; no bend.
        if (direction == newHeadDirection) { return; }

        //for bend sprite array
        //0 is rightUp, 1 is rightDown, 2 is leftUp, 3 is leftDown
        switch (direction)
        {
            case Direction.north:
                if (newHeadDirection == Direction.east)
                    sr.sprite = bodyBendsArray[3];
                else if (newHeadDirection == Direction.west)
                    sr.sprite = bodyBendsArray[1];
                break;
            case Direction.south:
                if (newHeadDirection == Direction.east)
                    sr.sprite = bodyBendsArray[2];
                else if (newHeadDirection == Direction.west)
                    sr.sprite = bodyBendsArray[0];
                break;
            case Direction.east:
                if (newHeadDirection == Direction.north)
                    sr.sprite = bodyBendsArray[0];
                else if (newHeadDirection == Direction.south)
                    sr.sprite = bodyBendsArray[1];
                break;
            case Direction.west:
                if (newHeadDirection == Direction.north)
                    sr.sprite = bodyBendsArray[2];
                else if (newHeadDirection == Direction.south)
                    sr.sprite = bodyBendsArray[3];
                break;
            default:
                break;
        }

    }

}
