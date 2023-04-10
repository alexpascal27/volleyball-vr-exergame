using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PongBlockLoop : MonoBehaviour
{
    public GameObject pongGameManagerPrefab;
    public GameObject targetPrefab;
    public float hitRegistrationCooldown = 0.5f;
    public float predictionDelay = 0.1f;
   
    private Rigidbody rb;
    private PongGameManager pongGameManager;
    private bool incremented = false;
    // make sure no other hit registrations for X sec
    private float coolDownTimer = 0f;
    private float delayTimer = 0f;
    private bool duringDelay = false;
    
    private void Start()
    {
       rb = GetComponent<Rigidbody>();
       pongGameManager = pongGameManagerPrefab.GetComponent<PongGameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Hand") && coolDownTimer <= 0)
        {
            duringDelay = true;
            delayTimer = predictionDelay;
        }
        // If hit floor on opponentSide
        else if (go.CompareTag("Court"))
        {
            if (!incremented)
            {
                pongGameManager.IncrementUserPoints();
                incremented = true;
            }
            // TODO: particles
            Destroy(gameObject);
        }
        else if (!go.CompareTag("Net") && transform.position.y < 0.25f)
        {
            if (!incremented)
            {
                pongGameManager.IncrementOpponentPoints();
                incremented = true;
            }
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (coolDownTimer > 0) coolDownTimer -= Time.deltaTime;
        if (delayTimer > 0) delayTimer -= Time.deltaTime;
        if (delayTimer <= 0 && duringDelay)
        {
            duringDelay = false;
            PredictAndSpawn();
        }
    }

    void PredictAndSpawn()
    {
        coolDownTimer = hitRegistrationCooldown;
        Vector3 predictedPosition = PredictPositionGivenY(0.5f);
        // only spawn receiver on opponent side of court
        if(!targetPrefab.IsUnityNull() && predictedPosition.z > 0.05f) 
            Instantiate(targetPrefab, predictedPosition, targetPrefab.transform.rotation);
    }
    
    Vector3 PredictPositionGivenY( float yCoordinate)
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
