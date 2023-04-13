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
    private KillAfterXSec killAfterXSec;
    
    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        killAfterXSec = GetComponent<KillAfterXSec>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionPrefab = collision.gameObject;
        if (collisionPrefab.CompareTag("Hand"))
        {
            if (!touchedPlayer)
            {
                touchedPlayer = true;
                sphereCollider.material = materialAfterCollision;
                killAfterXSec.enabled = true;
            }
        }
    }
}
