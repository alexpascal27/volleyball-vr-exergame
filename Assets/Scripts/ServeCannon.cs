using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeCannon : MonoBehaviour
{
    [SerializeField] public float maxPower = 10f;
    // Whether to sometimes slow down speed
    [SerializeField] public bool toRetardPower = false;
    // Randomise offset percentage to figure out how much to retard power by
    [SerializeField, Range(0f, 1f)] public float maxRetardOffset = 0f;
    [SerializeField] public GameObject ballPrefab;
    [SerializeField] public Vector3 defaultRotation = new Vector3(0f, 0f, 0f);
    [SerializeField] public Transform shotPoint;
    [SerializeField] public Vector3 maxSuccessAngle;
    [SerializeField] public Vector3 maxErrorAngle;
    [SerializeField, Range(0f, 1f)] public float percentageOfMisses = 0f;
    [SerializeField] public float delay = 2.0f;

    public GameObject targetPrefab;
    
    private float currTimeCounter;

    void Start()
    {
        currTimeCounter = delay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currTimeCounter <= 0f)
        {
            // Reset angle
            transform.rotation = Quaternion.Euler(defaultRotation);
            
            // Randomise angle
            bool miss = DetermineIfToMiss();
            Vector3 rotateVector = GenerateAngles(miss);
            // Rotate by rotateVector
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotateVector);
            // Spawn ball
            GameObject instantiatedBall = Instantiate(ballPrefab, shotPoint.position, shotPoint.rotation);
            float power = toRetardPower ? ApplyRetardation() : maxPower;
            
            Rigidbody rb = instantiatedBall.GetComponent<Rigidbody>();
            rb.velocity = shotPoint.transform.up * power;
            
            Vector3 predictedPosition = PredictPositionGivenY(rb, 0f);
            if(targetPrefab) Instantiate(targetPrefab, predictedPosition + new Vector3(0, 0.6f, 0), targetPrefab.transform.rotation);
            
            // reset Timer
            currTimeCounter = delay;
        }
        else currTimeCounter -= Time.deltaTime;
    }

    Vector3 GenerateAngles(bool miss)
    {
        float xAngle, yAngle, zAngle;
        if (miss)
        {
            int option = UnityEngine.Random.Range(0, 2);
            xAngle = UnityEngine.Random.Range(option == 0 ? -maxSuccessAngle.x - maxErrorAngle.x : 0f, option == 0 ? 0f : maxSuccessAngle.x + maxErrorAngle.x);
            option = UnityEngine.Random.Range(0, 2);
            yAngle = UnityEngine.Random.Range(option == 0 ? -maxSuccessAngle.y - maxErrorAngle.y : 0f, option == 0 ? 0f : maxSuccessAngle.y + maxErrorAngle.y);
            option = UnityEngine.Random.Range(0, 2);
            zAngle = UnityEngine.Random.Range(option == 0 ? -maxSuccessAngle.z - maxErrorAngle.z : 0f, option == 0 ? 0f : maxSuccessAngle.z + maxErrorAngle.z);
        }
        else
        {
            xAngle = UnityEngine.Random.Range(-maxSuccessAngle.x, maxSuccessAngle.x);
            yAngle = UnityEngine.Random.Range(-maxSuccessAngle.y, maxSuccessAngle.y);
            zAngle = UnityEngine.Random.Range(-maxSuccessAngle.z, maxSuccessAngle.z);
            
        }
        return new Vector3(xAngle, yAngle, zAngle);
    }
    
    bool DetermineIfToMiss()
    {
        // randomise between 0 and 1, and see if miss (between 0 and percentageOfMisses) or not
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        return randomValue <= percentageOfMisses;
    }

    float ApplyRetardation()
    {
        float offset = UnityEngine.Random.Range(0f, maxRetardOffset);
        return maxPower * (1f - offset);
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
}
