using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TictactoeTileController : MonoBehaviour
{
    [SerializeField] private GameObject xPrefab;
    [SerializeField] private GameObject oPrefab;

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
            TictactoeGridController tictactoeGridController = parentGridPrefab.GetComponent<TictactoeGridController>();

            // Grid Feedback
            bool isHit = tictactoeGridController.RegisterHit(tileName);
            // Spawn X or O
            bool isUserX = tictactoeGridController.GetIsUserX();
            Instantiate(isUserX ? xPrefab : oPrefab, transform.position, xPrefab.transform.rotation);

            tileHit = true;
        }
    }
}
