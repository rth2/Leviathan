using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterMovement : MonoBehaviour
{
    [SerializeField] private Critter critter = null;

    private int x = 0;

    private void Start()
    {
        StartCoroutine(Movement());
    }

    IEnumerator Movement()
    {
        

        while(true)
        {
            if (critter == null) yield return new WaitForSeconds(1.0f);

            transform.position += critter.GetDirection();
            yield return new WaitForSeconds(1.0f);
        }
        
    }
}
