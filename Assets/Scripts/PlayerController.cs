using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float playerSpeed = 3f;
    public SteamVR_Input_Sources handType;
    public Vector3 limitFromBase;
    public bool constraintX = false;
    public bool constraintZ = false;
    private SteamVR_Action_Vector2 moveAction;
    
    // Start is called before the first frame update
    void Start()
    {
        moveAction = SteamVR_Actions.default_Move;
        limitFromBase = new Vector3(Mathf.Abs(limitFromBase.x), Mathf.Abs(limitFromBase.y), Mathf.Abs(limitFromBase.z));
    }

    // Update is called once per frame
    void Update()
    {
        // x = horizontal, y = vertical
        Vector2 moveValue = moveAction.GetAxis(handType);
        
        Vector3 direction = new Vector3(constraintX ? 0 : moveValue.x, 0, constraintZ ? 0 : moveValue.y).normalized;
        Vector3 movement = transform.TransformDirection(direction) * (playerSpeed * Time.deltaTime);

        if(!OutOfRange(transform.position + movement)) transform.position += movement;
    }

    bool OutOfRange(Vector3 pos)
    {
        return pos.x < -limitFromBase.x || pos.x > limitFromBase.x || 
               pos.z < -limitFromBase.z || pos.z > 0;
    }
}
