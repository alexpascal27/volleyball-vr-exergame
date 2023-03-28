using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDisable : MonoBehaviour
{
    [SerializeField] public Collider[] collidersToDisable;
    [SerializeField] public Collider[] collidersToEnable;
    
    // Start is called before the first frame update
    void Start()
    {
        // Enable and Disable Colliders 
        foreach (Collider colliderToDisable in collidersToDisable)
        {
            colliderToDisable.enabled = false;
        }

        foreach (Collider colliderToEnable in collidersToEnable)
        {
            colliderToEnable.enabled = true;
        }
    }
}
