using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PongBlockLoop : MonoBehaviour
{
    public GameObject targetPrefab;
    public float hitRegistrationCooldown = 0.5f;
   
    private Rigidbody rigidbody;
    // make sure no other hit registrations for X sec
    private float timer;
    
    private void Start()
    {
       rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Hand") && timer <= 0)
        {
            timer = hitRegistrationCooldown;
            Vector3 predictedPosition = PredictPositionGivenY(0.5f);
            // only spawn receiver on opponent side of court
            if(!targetPrefab.IsUnityNull() && predictedPosition.z < 0) Instantiate(targetPrefab, predictedPosition + new Vector3(0, 0.6f, 0), targetPrefab.transform.rotation);
        }
    }

    private void FixedUpdate()
    {
        if (timer > 0) timer -= Time.deltaTime;
    }

    Vector3 PredictPositionGivenY( float yCoordinate)
    {
        Vector3 r0 = rigidbody.position;
        Vector3 v0 = rigidbody.velocity;
        Vector3 a = new Vector3(0, -9.8f, 0);
        float t0 = (-v0.y + Mathf.Sqrt(Mathf.Pow(v0.y, 2) - 2 * a.y * (r0.y - yCoordinate))) / a.y;
        float t1 = (-v0.y - Mathf.Sqrt(Mathf.Pow(v0.y, 2) - 2 * a.y * (r0.y - yCoordinate))) / a.y;
        
        // need t1
        if(t0 < 0 && t1 > 0)
        {
            return r0 + v0 * t1 + a * Mathf.Pow(t1, 2) / 2;
        }
        // need t0
        if(t0 > 0 && t1 < 0)
        {
            return r0 + v0 * t0 + a * Mathf.Pow(t1, 2) / 2;
        }
        
        // something wrong
        Debug.LogError("BOTH T0 and T1 below 0:     t0: " + t0 + "    And  t1: " + t1);
        return r0;
    }
}
