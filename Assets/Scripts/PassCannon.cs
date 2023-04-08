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
    // Outside position = Vector3(3.8, 3.2, 0.5)
    // Hitter highest hit while hitting ^ = Vector3(4, 1.5, 1.08)
    [SerializeField] public Vector3 basePassTargetPoint = new Vector3(0, 3.2f, 0.5f);
    [SerializeField] public Vector3 leewayFromBaseTargetPoint = new Vector3(3.8f, 0, 0);
    [SerializeField] public float timeToGetToTarget;

    public GameObject targetPrefab;
    public GameObject hitterPrefab;
    // a bit to the left, much lower and behind
    public Vector3 hitterPositionOffset = new Vector3(0.4f, -1.6f, 0.5f);

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
        Vector3 targetPoint = GenerateTargetPointAlongNet();
        Vector3 predictedVelocity = PredictVelocityGivenFinalPosition(rb, targetPoint, timeToGetToTarget);
        rb.velocity = predictedVelocity;
        
        Instantiate(targetPrefab, targetPoint, targetPrefab.transform.rotation);
        Instantiate(hitterPrefab, targetPoint + hitterPositionOffset, hitterPrefab.transform.rotation);
    }

    Vector3 GenerateTargetPointAlongNet()
    {
        Vector3 offset = new Vector3(
            UnityEngine.Random.Range(-leewayFromBaseTargetPoint.x, leewayFromBaseTargetPoint.x),
            UnityEngine.Random.Range(-leewayFromBaseTargetPoint.y, leewayFromBaseTargetPoint.y),
            UnityEngine.Random.Range(-leewayFromBaseTargetPoint.z, leewayFromBaseTargetPoint.z));
        return basePassTargetPoint + offset;
    }

    Vector3 PredictVelocityGivenFinalPosition(Rigidbody rb, Vector3 r, float t)
    {
        // v0 = (r - r0 - 1/2at^2) / t              = rearrangement of kinematic equation
        Vector3 r0 = rb.position;
        Vector3 a = new Vector3(0, -9.8f, 0);

        return (r - r0 - (a * Mathf.Pow(t, 2)) / 2) / t;
    }
}
