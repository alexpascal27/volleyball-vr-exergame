using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHitController : MonoBehaviour
{
    [SerializeField] public GameObject linePrefab;
    // we want to apply a force upon a collision with ball
    [SerializeField] private float forceScale = 100f;
    [SerializeField] public Collider[] collidersToDisable;
    [SerializeField] public Collider[] collidersToEnable;
    
    private Vector3 knockbackDirection;

    private Rigidbody rigidbody;


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
        rigidbody = GetComponent<Rigidbody>();

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
        String tag = collision.gameObject.tag;
        if (tag == "Ball")
        {
            //Debug.Log("TouchingBall");
            Debug.Log(rigidbody==null ? "rigidbody null" : "rigidbody defined|Position = " + rigidbody.position);
            
            // find direction of knockback
            ContactPoint contact = collision.contacts[0];
            knockbackDirection = contact.point - transform.position;
            knockbackDirection = knockbackDirection.normalized;
            
            // spawn line
            linePrefab.transform.LookAt(knockbackDirection);
            Instantiate(linePrefab, contact.point, linePrefab.transform.rotation);
            
            //ApplyKnockBack();
        }
    }

    private void ApplyKnockBack()
    {
        rigidbody.AddForce(knockbackDirection * forceScale, ForceMode.Impulse);
    }
}
