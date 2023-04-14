using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleshipTileController : MonoBehaviour
{
    [SerializeField] private Material successfulHitMaterial;
    [SerializeField] private Material failedHitMaterial;
    [SerializeField] private Material shipSunkHitMaterial;
    
    private bool tileHit = false;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (tileHit) return;

        GameObject tilePrefab = gameObject;
        // When collide with ball
        String collisionGameObjectTag = collision.gameObject.tag;
        if (collisionGameObjectTag == "Ball")
        {
            CountTileCollisions countTileCollisions = collision.gameObject.GetComponent<CountTileCollisions>();
            if (countTileCollisions.hasAlreadyHitADifferentTile) return;
            
            // Get parent (row name e.g. A)
            GameObject parentRowPrefab = tilePrefab.transform.parent.gameObject;
            String rowName = parentRowPrefab.name;
            // Create tileName = {rowName}{tileNumber}
            String tileName = rowName + tilePrefab.name;
            // Get parent (grid)
            GameObject parentGridPrefab = parentRowPrefab.transform.parent.gameObject;
            // Call RegisterGrid
            BattleshipGridController battleshipGridController = parentGridPrefab.GetComponent<BattleshipGridController>();
            
            // Tile Feedback
            bool isHit = battleshipGridController.RegisterHit(tileName, transform.position);
            ChangeTileMaterial(isHit);
            tileHit = true;
        }
    }

    private void ChangeTileMaterial(bool isHit)
    {
        // Turn on renderer
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.enabled = true;
        // if isHit turn material to successfulHitMaterial
        if (isHit)
        {
            renderer.material = successfulHitMaterial;
        }
        // else turn material to failedHitMaterial
        else
        {
            renderer.material = failedHitMaterial;
        }
    }

    public void ChangeTileMaterialShipSunk()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        
        renderer.material = shipSunkHitMaterial;
    }
}
