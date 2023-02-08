using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallServe : MonoBehaviour
{
    [SerializeField] public GameObject ballPrefab;
    [SerializeField, Range(0f, 1f)] public float percentageOfMisses = 0f;
    [SerializeField] public float outOfCourtRange = 5f;
    [SerializeField] public float netHeight = 10f;
    [SerializeField] public float forceScale = 100f;
    [SerializeField] public float delay = 2.0f;
    [SerializeField] public Tuple<float, float> courtOrigin = new Tuple<float, float>(0f,0f);
    [SerializeField] public Tuple<float, float> courtDimensions;

    private float currTimeCounter;
    private Rigidbody spawnPointRigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnPointRigidbody = GetComponent<Rigidbody>();
        StartCoroutine(SpawnBallWithDelay());
    }

    IEnumerator SpawnBallWithDelay()
    {
        yield return new WaitForSeconds(delay);
        
        bool miss = DetermineIfToMiss();
        Vector3 target = GenerateTarget(miss);
        ballPrefab.transform.LookAt(target);
        // TODO: investigate if to apply acceleration or impulse
        spawnPointRigidbody.AddForce(ballPrefab.transform.forward * forceScale, ForceMode.Impulse);
        
        Debug.Log("Ball Spawned");
    }

    bool DetermineIfToMiss()
    {
        // randomise between 0 and 1, and see if miss (between 0 and percentageOfMisses) or not
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        return randomValue <= percentageOfMisses;
    }

    Vector3 GenerateTarget(bool miss)
    {
        float z = netHeight;
        
        if (miss)
        {
            /* for origin point (X, Y) and dimensions (M, N) and outOfCourtRange R, we have the available range of:
             * BEHIND: ({X - M/2 - R, X + M/2 + R}, {Y + N, Y + N + R}) OR
             * LEFT: ({X - M/2 - R, X - M/2}, {Y, Y+N}) OR
             * RIGHT: ({X + M/2, X + M/2 + R}, {Y, Y+N})
             */
            
            // see if left(0), right(1) or behind(2)
            int option = UnityEngine.Random.Range(0, 3);
            if (option == 0)
            {
                float x = UnityEngine.Random.Range(courtOrigin.Item1 - courtDimensions.Item1/2f - outOfCourtRange, courtOrigin.Item1 - courtDimensions.Item1/2f);
                float y = UnityEngine.Random.Range(courtOrigin.Item2, courtOrigin.Item2 + courtDimensions.Item2);
                return new Vector3(x, y, z);
            }
            else if (option == 1)
            {
                float x = UnityEngine.Random.Range(courtOrigin.Item1 + courtDimensions.Item1/2f, courtOrigin.Item1 + courtDimensions.Item1/2f + outOfCourtRange);
                float y = UnityEngine.Random.Range(courtOrigin.Item2, courtOrigin.Item2 + courtDimensions.Item2);
                return new Vector3(x, y, z);
            }
            else
            {
                float x = UnityEngine.Random.Range(courtOrigin.Item1 - courtDimensions.Item1/2f - outOfCourtRange, courtOrigin.Item1 + courtDimensions.Item1/2f + outOfCourtRange);
                float y = UnityEngine.Random.Range(courtOrigin.Item2 + courtDimensions.Item2, courtOrigin.Item2 + courtDimensions.Item2 + outOfCourtRange);
                return new Vector3(x, y, z);
            }

        }
        else
        {
            // for origin point (X, Y) and dimensions (M, N) we have the available range of ({X - M/2, X + M/2}, {Y, Y + N}) 
            float x = UnityEngine.Random.Range(courtOrigin.Item1 - courtDimensions.Item1/2f, courtOrigin.Item1 + courtDimensions.Item1/2f);
            float y = UnityEngine.Random.Range(courtOrigin.Item2, courtOrigin.Item2 + courtDimensions.Item2);

            return new Vector3(x, y, z);
        }
    }
}
