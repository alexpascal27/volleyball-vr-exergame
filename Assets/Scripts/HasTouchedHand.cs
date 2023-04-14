using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasTouchedHand : MonoBehaviour
{
    public bool hasTouchedHand = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasTouchedHand)
        {
            if (collision.gameObject.CompareTag("Hand"))
            {
                hasTouchedHand = true;
            }
        }
        
    }
}
