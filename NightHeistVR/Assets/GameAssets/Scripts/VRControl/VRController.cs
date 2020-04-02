using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRController : MonoBehaviour
{
    public float gravity = 30.0f;
    public float sensitivity = 0.1f;
    public float maxSpeed = 1.0f;
    public float rotateIncrement = 45;
    
    public SteamVR_Action_Vector2 joystick;
    private SteamVR_Input_Sources rightHandInput = SteamVR_Input_Sources.RightHand;
    private SteamVR_Input_Sources leftHandInput = SteamVR_Input_Sources.LeftHand;

    private float speed = 0.0f;

    private CharacterController characterController = null;
    private Transform cameraRig = null;
    private Transform head = null;

    private bool rotated;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    private void Start() {
        cameraRig = SteamVR_Render.Top().origin;
        head = SteamVR_Render.Top().head;
    }

    private void Update() {
        HandleHeight();
        Move();
        SnapRotation();
    }

    private void HandleHeight() {
        float headHeight = Mathf.Clamp(head.localPosition.y, 1, 2);
        characterController.height = headHeight;

        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y += characterController.skinWidth;

        newCenter.x = head.localPosition.x;
        newCenter.z = head.localPosition.z;

        characterController.center = newCenter;
    }

    private void Move() {
        Quaternion orientation = Orientation();
        Vector3 movement = Vector3.zero;

        if(joystick.GetAxis(leftHandInput).magnitude == 0) {
            speed = 0;
        }
        
        speed += joystick.GetAxis(leftHandInput).magnitude * sensitivity;
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

        movement += orientation * (speed * Vector3.forward);
        movement.y -= gravity * Time.deltaTime;

        characterController.Move(movement * Time.deltaTime);
    }

    private Quaternion Orientation() {
        float rotation = Mathf.Atan2(joystick.GetAxis(leftHandInput).x, joystick.GetAxis(leftHandInput).y);
        rotation *= Mathf.Rad2Deg;

        Vector3 orientationEuler = new Vector3(0, head.eulerAngles.y + rotation, 0);
        return Quaternion.Euler(orientationEuler);
    }

    private void SnapRotation() {
        float snapValue = 0.0f;

        if (rotated) {
            if(joystick.GetAxis(rightHandInput).x >= -0.1f && joystick.GetAxis(rightHandInput).x <= 0.1f) {
                rotated = false;
            }
        } else {
            if (joystick.GetAxis(rightHandInput).x >= 0.9f) {
                snapValue = Mathf.Abs(rotateIncrement);
                rotated = true;
            }
            if (joystick.GetAxis(rightHandInput).x <= -0.9f) {
                snapValue = -Mathf.Abs(rotateIncrement);
                rotated = true;
            }
        }

        transform.RotateAround(head.position, Vector3.up, snapValue);
    }
}
