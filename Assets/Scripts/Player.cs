using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField] Critter critter = null;
    [SerializeField] GameLoop gameLoop = null;

    Vector2 rawInput = new Vector2();
    Vector3 sanitizedInput = new Vector3();
    Camera mainCamera = null;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMoveWASD(InputValue value)
    {
        if(critter == null) { return; }

        rawInput = value.Get<Vector2>();

        //i'm only sending in directions left, right, up, down.
        if (rawInput.x != 0 && rawInput.y != 0) { return; }
        if(rawInput.x == 0 && rawInput.y == 0) { return; }

        //this is because my 2d array is built from the upper left.
        //have to flip the y to make the directions work or build grid from bottom left.
        rawInput.y = -rawInput.y;

        critter.SetDirection(new Vector2(rawInput.x, rawInput.y));

    }

    void OnMoveTouch(InputValue value)
    {
        if (critter == null) { return; }

        rawInput = value.Get<Vector2>();

        sanitizedInput = mainCamera.ScreenToWorldPoint(new Vector3(rawInput.x, rawInput.y, 0));

        critter.SetDirectionTouch(new Vector2(sanitizedInput.x, sanitizedInput.y));
    }

    private void OnPause()
    {
        if(gameLoop == null) { return; }
        gameLoop.HandlePause();
    }

    

}
