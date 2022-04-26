using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter : MonoBehaviour
{
    [SerializeField] private int curLength = 2;
    [SerializeField] private Vector3 critterDirection = new Vector3();

    private void Awake()
    {
        critterDirection = Vector3.left;
    }

    public int GetLength()
    {
        return curLength;
    }

    public Vector3 GetDirection()
    {
        return critterDirection;
    }

}
