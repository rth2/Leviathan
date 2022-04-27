using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterMovement : MonoBehaviour
{
    [SerializeField] private Critter critter = null;
    [SerializeField] private GameLoop gameLoop = null;

    private void Start()
    {
        if (gameLoop == null) { return; }

        gameLoop.OnNewTickCycle += Movement;
    }

    private void Movement()
    {
        if (critter == null) { return; }

        //transform.position += critter.GetDirection();
        //Debug.Log($"{transform.position}");
 
    }
}
