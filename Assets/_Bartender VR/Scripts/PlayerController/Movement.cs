using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Movement : MonoBehaviour
{
    public SteamVR_Action_Vector2 joystickAction;
    public SteamVR_Input_Sources MoveHand;
    public SteamVR_Input_Sources RotateHand;
    public float TurnAmount = 4.0f;
    public float MoveSpeed = 0.5f;
    public GameObject HeadProxy;
    public GameObject Camera;
    public GameObject Offset;

    private bool Tick = false;
    private Vector3 Rotation = Vector3.zero;
    private Vector3 Position = new Vector3(0, 0.626f, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Offset.transform.localPosition = new Vector3(-Camera.transform.localPosition.x, 0, -Camera.transform.localPosition.z);

        HeadProxy.transform.position = Camera.transform.position;
        HeadProxy.transform.rotation = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);

        Vector2 moveV = joystickAction.GetAxis(MoveHand);
        Vector2 rotateV = joystickAction.GetAxis(RotateHand);

        if (rotateV.x >= 0.001f && !Tick)
        {
            Tick = true;
            Rotation += Vector3.up * TurnAmount;
        }
        else if (rotateV.x <= -0.001f && !Tick)
        {
            Tick = true;
            Rotation += Vector3.up * -TurnAmount;
        }
        else if(rotateV.x == 0) Tick = false;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Rotation), 0.1f);

        if (moveV.x > 0.001f) Position += HeadProxy.transform.right * MoveSpeed;
        if (moveV.x < -0.001f) Position -= HeadProxy.transform.right * MoveSpeed;
        if (moveV.y > 0.001f) Position += HeadProxy.transform.forward * MoveSpeed;
        if (moveV.y < -0.001f) Position -= HeadProxy.transform.forward * MoveSpeed;

        Position = new Vector3(Mathf.Clamp(Position.x, -2.5f, 2.5f), Position.y, Mathf.Clamp(Position.z, -0.5f, 1.05f));
        transform.position = Vector3.Lerp(transform.position, Position, 0.1f);
    }
}