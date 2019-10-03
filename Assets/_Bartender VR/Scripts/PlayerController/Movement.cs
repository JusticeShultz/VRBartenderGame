using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Movement : MonoBehaviour
{
    public static GameObject Player;
    public static Movement Instance;

    public SteamVR_Action_Boolean crouchToggle;
    public SteamVR_Action_Vector2 joystickAction;

    public SteamVR_Input_Sources MoveHand;
    public SteamVR_Input_Sources RotateHand;
    public SteamVR_Input_Sources CrouchHand;

    public float TurnAmount = 4.0f;
    public float MoveSpeed = 0.5f;
    public float StandingY = 0.626f;
    public float CrouchingY = 0.25f;

    public GameObject HeadProxy;
    public GameObject Camera;
    public GameObject Offset;

    public Vector2 LeftRightMinMax;
    public Vector2 ForwardBackwardMinMax;

    private Vector3 Rotation = Vector3.zero;
    private Vector3 Position = new Vector3(0, 0, 0);

    private bool Tick = false;
    private bool Crouching = false;

    private void Start()
    {
        Player = gameObject;
        Instance = this;
    }

    void Update()
    {
        if(crouchToggle.GetChanged(CrouchHand))
            Crouching = !Crouching;

        //Offset.transform.localPosition = new Vector3(-Camera.transform.localPosition.x, 0, -Camera.transform.localPosition.z);

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

        Offset.transform.rotation = Quaternion.Lerp(Offset.transform.rotation, Quaternion.Euler(Rotation), 0.1f);

        if (moveV.x > 0.001f) Position += HeadProxy.transform.right * MoveSpeed;
        if (moveV.x < -0.001f) Position -= HeadProxy.transform.right * MoveSpeed;
        if (moveV.y > 0.001f) Position += HeadProxy.transform.forward * MoveSpeed;
        if (moveV.y < -0.001f) Position -= HeadProxy.transform.forward * MoveSpeed;

        if(!Crouching)
             Position = new Vector3(Mathf.Clamp(Position.x, LeftRightMinMax.x, LeftRightMinMax.y), StandingY, Mathf.Clamp(Position.z, ForwardBackwardMinMax.x, ForwardBackwardMinMax.y));
        else Position = new Vector3(Mathf.Clamp(Position.x, LeftRightMinMax.x, LeftRightMinMax.y), CrouchingY, Mathf.Clamp(Position.z, ForwardBackwardMinMax.x, ForwardBackwardMinMax.y));

        transform.position = Vector3.Lerp(transform.position, Position, 0.1f);
    }
}