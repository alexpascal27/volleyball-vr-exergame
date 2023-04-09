using System.Collections;
using UnityEngine;
using Valve.VR;

public class PlayerJumpController : MonoBehaviour
{
    public SteamVR_Input_Sources oneHandType;
    public SteamVR_Input_Sources otherHandType;
    public SteamVR_Action_Boolean triggerHoldAction;
    public Rigidbody playerRb;
    public float groundHeight = 0f;
    public float maxHeight = 2f;
    public float jumpRate = 1f;
    
    private bool holdingBothTriggers = false;
    private bool holdTriggerLoop = false;
    private bool jumping = false;
    private Coroutine jumpCoroutine;

    void Update()
    {
        // Updating holdingBothTriggers boolean
        if (triggerHoldAction.GetState(oneHandType) && triggerHoldAction.GetState(otherHandType)) holdingBothTriggers = true;
        else holdingBothTriggers = false;

        // to avoid loop where you hold down triggers, so you infinitely jump, reset and jump
        if(!holdingBothTriggers && holdTriggerLoop) holdTriggerLoop = false;
        
        float currPlayerHeight = playerRb.position.y;
        // Only start jumping if on ground, not jumping and holding both triggers
        if (holdingBothTriggers && !jumping && currPlayerHeight <= groundHeight && !holdTriggerLoop)
        {
            jumpCoroutine = StartCoroutine(JumpCoroutine());
            Debug.Log("Holding both triggers, on ground and not jumping");
        }
        // If reached max height or leg go of triggers
        if (currPlayerHeight >= maxHeight || (currPlayerHeight > groundHeight && !holdingBothTriggers && jumping))
        {
            StopCoroutine(jumpCoroutine);
            jumping = false;
            Debug.Log("Already in air, now droppping");
            transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
            if (holdingBothTriggers) holdTriggerLoop = true;
        }
    }
    
    IEnumerator JumpCoroutine()
    {
        jumping = true;
        
        float startHeight = groundHeight;
        float targetHeight = maxHeight;
        float currPlayerHeight = playerRb.position.y;

        while (currPlayerHeight < targetHeight)
        {
            currPlayerHeight += Time.deltaTime * jumpRate;
            transform.position = new Vector3(transform.position.x, currPlayerHeight, transform.position.z);
            yield return null;
        }
    }
}
