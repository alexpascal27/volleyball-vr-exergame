using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowServePassFail : MonoBehaviour
{
    [SerializeField] public GameObject targetHighlightPrefab;
    [SerializeField] public Material successMaterial;
    [SerializeField] public Material failMaterial;

    // only need to check once
    private int collisionCount = 0;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collisionCount <= 0)
        {
            String collisionObjectTag = collision.gameObject.tag;
            
            if (collisionObjectTag is "Hand" or "Net") return;
            
            Vector3 collisionPoint = collision.GetContact(0).point;
            Vector3 spawnPoint = new Vector3(collisionPoint.x, collisionPoint.y + 0.1f, collisionPoint.z);
            GameObject spawnedInstance = Instantiate(targetHighlightPrefab, spawnPoint, targetHighlightPrefab.transform.rotation);
            Renderer targetRenderer = spawnedInstance.GetComponent<Renderer>();
            if (collisionObjectTag == "Court")
            {
                targetRenderer.material = successMaterial;
            }
            else
            {
                targetRenderer.material = failMaterial;
            }

            collisionCount++;
        }
        
    }
}
