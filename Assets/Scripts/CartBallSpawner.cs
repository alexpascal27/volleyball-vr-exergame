using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class CartBallSpawner : MonoBehaviour
{
    [SerializeField] private float upwardsOffsetSpawnPoint = 0.5f;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private float timeInBetweenCheck = 2.0f;
    [SerializeField] private float raycastLength =  2.0f;
    [SerializeField] private bool conditionalOnRaycast = true;

    private RaycastHit hit;
    private float currTimeCounter;
    private Vector3 spawnPoint;

    private void Start()
    {
        currTimeCounter = timeInBetweenCheck;
        // above transform position
        spawnPoint = transform.position + upwardsOffsetSpawnPoint * Vector3.up;
    }

    private void FixedUpdate()
    {
        if (currTimeCounter <= 0.0f)
        {
            // fire ray
            bool hitSomething = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit,
                raycastLength);
            // if not hit, spawn
            if (!hitSomething || !conditionalOnRaycast)
            {
                Instantiate(objectToSpawn, spawnPoint, transform.rotation);
            }

            // reset Timer
            currTimeCounter = timeInBetweenCheck;
        }
        else currTimeCounter -= Time.deltaTime;
    }
}
