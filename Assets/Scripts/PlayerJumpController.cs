using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerJumpController : MonoBehaviour
{
    public SteamVR_Input_Sources oneHandType;
    public SteamVR_Input_Sources otherHandType;
    public SteamVR_Action_Boolean triggerHoldAction;

    // Update is called once per frame
    void Update()
    {
        if (triggerHoldAction.GetState(oneHandType) && triggerHoldAction.GetState(otherHandType))
        {
            Debug.Log("Holding both hand triggers");
        }
    }
}
