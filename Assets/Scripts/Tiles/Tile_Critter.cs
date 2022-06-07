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

    Direction direction;


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
    }

    public void SetSpriteHead()
    {
        sr.sprite = headSpriteArray[(int)direction];
    }

}
