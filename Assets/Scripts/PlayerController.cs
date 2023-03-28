using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float playerSpeed = 3f;
    public SteamVR_Input_Sources handType;
    private SteamVR_Action_Vector2 moveAction;
    
    // Start is called before the first frame update
    void Start()
    {
        moveAction = SteamVR_Actions.default_Move;
    }

    // Update is called once per frame
    void Update()
    {
        // x = horizontal, y = vertical
        Vector2 moveValue = moveAction.GetAxis(handType);
        
        Vector3 direction = new Vector3(moveValue.x, 0, moveValue.y).normalized;
        Vector3 movement = transform.TransformDirection(direction) * (playerSpeed * Time.deltaTime);

        transform.position += movement;
    }
}
