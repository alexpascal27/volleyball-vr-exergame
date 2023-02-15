using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountTileCollisions : MonoBehaviour
{
    public bool hasAlreadyHitADifferentTile = false; 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grid")) hasAlreadyHitADifferentTile = true;
    }
}
