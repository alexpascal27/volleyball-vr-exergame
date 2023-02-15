using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountTileCollisions : MonoBehaviour
{
    public bool hasAlreadyHitADifferentTile = false;
    private String prevTileName = "";
    
    private void OnCollisionEnter(Collision collision)
    {
        GameObject tilePrefab = collision.gameObject;
        if (tilePrefab.CompareTag("Grid"))
        {
            GameObject parentRowPrefab = tilePrefab.transform.parent.gameObject;
            String rowName = parentRowPrefab.name;
            // Create tileName = {rowName}{tileNumber}
            String tileName = rowName + tilePrefab.name;
            // Only true if hit a tile before and 
            if (prevTileName == "")
            {
                prevTileName = tileName;
            }
            // touching a new one now
            if (prevTileName != "" && tileName != prevTileName)
            {
                hasAlreadyHitADifferentTile = true; 
            }
        }
    }
}
