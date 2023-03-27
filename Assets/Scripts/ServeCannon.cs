using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeCannon : MonoBehaviour
{
    [SerializeField] public float power = 10f;
    [SerializeField] public GameObject ballPrefab;
    [SerializeField] public Transform shotPoint;
    [SerializeField] public Vector3 maxSuccessAngle;
    [SerializeField] public Vector3 maxErrorAngle;
    [SerializeField, Range(0f, 1f)] public float percentageOfMisses = 0f;
    [SerializeField] public float delay = 2.0f;
    
    private float currTimeCounter;

    void Start()
    {
        currTimeCounter = delay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 rotateVector = new Vector3(0f, 0f, 0f);
        if (currTimeCounter <= 0f)
        {
            // Reset angle
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            
            // Randomise angle
            bool miss = DetermineIfToMiss();
            rotateVector = GenerateAngles(miss);
            // Rotate by rotateVector
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotateVector);
            // Spawn ball
            GameObject instatiatedBall = Instantiate(ballPrefab, shotPoint.position, shotPoint.rotation);
            instatiatedBall.GetComponent<Rigidbody>().velocity = shotPoint.transform.up * power;
            
            
            
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
}
