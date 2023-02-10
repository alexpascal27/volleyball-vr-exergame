using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BallHitController : MonoBehaviour
{
    [SerializeField] public GameObject linePrefab;
    // we want to apply a force upon a collision with ball
    [SerializeField] private float forceScale = 100f;
    
    [SerializeField] public float yHitOffset = 3f;
    
    [SerializeField] public Collider[] collidersToDisable;
    [SerializeField] public Collider[] collidersToEnable;
    
    private Vector3 reflectionDirection;

    private Rigidbody controllerRigidbody;


    // This means on any collision with the ball it adds a forward force
    // This does not exclude a throw
    // TODO: when add functionality to toss and hit separately see below logic on how to work
    /*
     * If (holdingBall) addForce = false;
     * Else:
     *  If (previousHand == currentHand)
     *      {
     *          # wait X secs    
     *          wait(X);
     *          addForce = true;
     *      }
     *  Else addForce = true;
     */

    private void Start()
    {
        controllerRigidbody = GetComponent<Rigidbody>();
        
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

    private void OnCollisionEnter(Collision collision)
    {
        GameObject ballPrefab = collision.gameObject;
        String collisionTag = ballPrefab.tag;
        if (collisionTag == "Ball")
        {
            //Debug.Log("TouchingBall");
            // find direction of knockback
            ContactPoint contact = collision.contacts[0];
            reflectionDirection = controllerRigidbody.velocity.normalized;

            // spawn line
            linePrefab.transform.LookAt(reflectionDirection);
            Instantiate(linePrefab, contact.point, linePrefab.transform.rotation);
            
            ApplyHit(ballPrefab);
        }
    }

    // TODO: Get controller velocity to make hit power depend on controller velocity
    // This is an estimation on the max speed that an average person might reach while moving the controller
    // - used to calculate ratio of currSpeed/maxSpeed
    private void ApplyHit(GameObject ballPrefab)
    {
        //Vector3 target = new Vector3(reflectionDirection.x, reflectionDirection.y + yHitOffset, reflectionDirection.z);
        //ballPrefab.transform.LookAt(target);
        Rigidbody ballRigidbody = ballPrefab.GetComponent<Rigidbody>();
        Vector3 prevVelocity = ballRigidbody.velocity;
        prevVelocity = new Vector3(prevVelocity.x, prevVelocity.y + yHitOffset, prevVelocity.z);
        ballRigidbody.velocity = prevVelocity * forceScale;
    }
}
