using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class TictactoeTileController : MonoBehaviour
{
    public GameObject userTilesPrefab;
    [SerializeField] private GameObject xPrefab;
    [SerializeField] private GameObject oPrefab;
    public Vector3 oppositeSidePositionOffset;

    private bool tileHit = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (tileHit) return;

        GameObject tilePrefab = gameObject;
        // When collide with ball
        GameObject collisionGameObject = collision.gameObject;
        if (collisionGameObject.CompareTag("Ball"))
        {
            HasTouchedHand hasTouchedHand = collisionGameObject.GetComponent<HasTouchedHand>();
            bool hasBallTouchedHand = hasTouchedHand.hasTouchedHand;
            
            CountTileCollisions countTileCollisions = collisionGameObject.GetComponent<CountTileCollisions>();
            if (countTileCollisions.hasAlreadyHitADifferentTile) return;

            // Get parent (row name e.g. A)
            GameObject parentRowPrefab = tilePrefab.transform.parent.gameObject;
            String rowName = parentRowPrefab.name;
            // Create tileName = {rowName}{tileNumber}
            String tileName = rowName + tilePrefab.name;
            // Get parent (grid)
            GameObject entityPrefab = parentRowPrefab.transform.parent.gameObject;
            GameObject parentGridPrefab = entityPrefab.transform.parent.gameObject;
            // Call RegisterGrid
            TictactoeGridController tictactoeGridController = parentGridPrefab.GetComponent<TictactoeGridController>();

            // Grid Feedback
            bool isUserTile = entityPrefab.name=="User";
            bool alreadyHit = tictactoeGridController.RegisterHit(tileName, isUserTile, hasBallTouchedHand);
            if (alreadyHit) return;
            // Spawn X or O
            bool isUserX = tictactoeGridController.GetIsUserX();

            Vector3 transformPosition = transform.position;
            // spawn user side
            GameObject userPrefab = isUserX ? xPrefab : oPrefab;
            GameObject opponentPrefab = isUserX ? oPrefab : xPrefab;
            Instantiate(isUserTile ? userPrefab : opponentPrefab, transformPosition, xPrefab.transform.rotation, isUserTile ? userTilesPrefab.transform : parentGridPrefab.transform);
            // only spawn opponent side from user tiles
            Instantiate(isUserTile ? userPrefab : opponentPrefab, new Vector3(isUserTile? transformPosition.x + 15f : transformPosition.x - 15f, transformPosition.y, -transformPosition.z) + oppositeSidePositionOffset, xPrefab.transform.rotation, isUserX ? userTilesPrefab.transform : entityPrefab.transform);
            tileHit = true;
        }
    }
}
