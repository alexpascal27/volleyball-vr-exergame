using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCannon : MonoBehaviour
{
    [SerializeField] public float maxPower = 10f;
    [SerializeField] public GameObject ballPrefab;
    [SerializeField] public Vector3 defaultRotation = new Vector3(0f, 0f, 0f);
    [SerializeField] public Transform shotPoint;
    [SerializeField] public Vector3 passTargetPoint;
    [SerializeField] public float timeToGetToTarget;

    public GameObject targetPrefab;

    private BoxCollider boxCollider;
    private int counter = 0;
    
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Ball"))
        {
            if (counter == 0)
            {
                counter++;
                // Destroy ball and spawn new one to make receiving control easier
                Destroy(go);
                boxCollider.enabled = false;
                Shoot();
            }
            
        }
    }

    void Shoot()
    {
        // Spawn ball
        GameObject instantiatedBall = Instantiate(ballPrefab, shotPoint.position, Quaternion.Euler(Vector3.zero));
        Rigidbody rb = instantiatedBall.GetComponent<Rigidbody>();
        Vector3 predictedVelocity = PredictVelocityGivenFinalPosition(rb, passTargetPoint, timeToGetToTarget);
        rb.velocity = predictedVelocity;
        
        //Vector3 predictedPosition = PredictPositionGivenY(rb, 0f);
        //Instantiate(targetPrefab, predictedPosition, targetPrefab.transform.rotation);
    }

    // Assumes no obstacles in the way
    Vector3 PredictPositionGivenY(Rigidbody rb, float yCoordinate)
    {
        Vector3 r0 = rb.position;
        Vector3 v0 = rb.velocity;
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

    Vector3 PredictVelocityGivenFinalPosition(Rigidbody rb, Vector3 r, float t)
    {
        // v0 = (r - r0 - 1/2at^2) / t              = rearrangement of kinematic equation
        Vector3 r0 = rb.position;
        Vector3 a = new Vector3(0, -9.8f, 0);

        return (r - r0 - (a * Mathf.Pow(t, 2)) / 2) / t;
    }
}
