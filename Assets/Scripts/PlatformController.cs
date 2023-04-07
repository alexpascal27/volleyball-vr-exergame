using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public GameObject leftHandPrefab;
    public GameObject leftHandColliderPrefab;
    public GameObject rightHandPrefab;
    public GameObject rightHandColliderPrefab;
    public GameObject userGridPrefab;
    public GameObject userTilePrefab;
    [SerializeField] private float distanceBetweenHandsToTrigger = 1f;

    private bool inTriggerRange = false;

    private void Start()
    {
        userGridPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if left and right hand are X distance apart
        Vector3 leftHandPosition = leftHandPrefab.transform.position;
        Vector3 rightHandPosition = rightHandPrefab.transform.position;
        float distanceApart = Vector3.Distance(leftHandPosition, rightHandPosition);
        // if no longer in range:      inRange and now distance is too large uncheck inRange bool
        if (inTriggerRange && distanceApart > distanceBetweenHandsToTrigger)
        {
            inTriggerRange = false;
            userGridPrefab.SetActive(false);
            ChangeTileValuesVisibility(false);
            SeparateHandColliders();
        }
        // if fresh in range:      not inRange and distance small enough, check 
        if (!inTriggerRange && distanceApart <= distanceBetweenHandsToTrigger)
        {
            inTriggerRange = true;
            userGridPrefab.SetActive(true);
            ChangeTileValuesVisibility(true);
            CombineHandColliders();
        }
    }

    void ChangeTileValuesVisibility(bool visible)
    {
        int childCount = userTilePrefab.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            GameObject child = userTilePrefab.transform.GetChild(i).gameObject;
            Renderer[] renderers = child.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = visible;
            }
        }
    }

    void CombineHandColliders()
    {
        BoxCollider leftHandBoxCollider = GetLeftHandCollider();
        BoxCollider rightHandBoxCollider = GetRightHandCollider();
        // Disable left collider
        leftHandBoxCollider.enabled = false;
        // Find difference between colliders
        Vector3 posDifference = rightHandBoxCollider.transform.position - leftHandColliderPrefab.transform.position;
        posDifference /= 2;
        // Offset right collider to midpoint between hand colliders
        rightHandBoxCollider.center = posDifference;
        // Double hand collider size
        rightHandBoxCollider.size *= 2;
    }

    void SeparateHandColliders()
    {
        BoxCollider leftHandBoxCollider = GetLeftHandCollider();
        BoxCollider rightHandBoxCollider = GetRightHandCollider();
        // Half right collider size
        rightHandBoxCollider.size /= 2;
        // offset set to 0
        rightHandBoxCollider.center = Vector3.zero;
        // enable left collider
        leftHandBoxCollider.enabled = true;
    }

    BoxCollider GetLeftHandCollider()
    {
        return leftHandPrefab.GetNamedChild("receivePlatform").GetComponent<BoxCollider>();
    }
    
    BoxCollider GetRightHandCollider()
    {
        return rightHandPrefab.GetNamedChild("receivePlatform").GetComponent<BoxCollider>();
    }
}
