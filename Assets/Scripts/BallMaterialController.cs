using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class BallMaterialController : MonoBehaviour
{
    public PhysicMaterial materialAfterCollision;
    
    private bool touchedPlayer = false;
    private SphereCollider sphereCollider;
    
    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionPrefab = collision.gameObject;
        if (collisionPrefab.CompareTag("Hand"))
        {
            if (!touchedPlayer)
            {
                Debug.Log("Touched hand for first time");
                touchedPlayer = true;
                sphereCollider.material = materialAfterCollision;
            }
        }
    }
}
