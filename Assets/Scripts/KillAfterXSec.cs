using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAfterXSec : MonoBehaviour
{
    [SerializeField] private float killDelay = 2f;
    
    void Start()
    {
        Destroy(gameObject, killDelay);
    }
}
