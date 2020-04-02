using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRController_Backup : MonoBehaviour
{
    public float sensitivity = 0.1f;
    public float maxSpeed = 1.0f;
    
    public SteamVR_Action_Vector2 joystick;
    private SteamVR_Input_Sources leftHandInput = SteamVR_Input_Sources.LeftHand;

    private Vector3 speed = Vector3.zero;

    private CharacterController characterController = null;
    private Transform cameraRig = null;
    private Transform head = null;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    private void Start() {
        cameraRig = SteamVR_Render.Top().origin;
        head = SteamVR_Render.Top().head;

        joystick.AddOnAxisListener(Move, leftHandInput);
    }

    private void Update() {
        HandleHead();
        HandleHeight();
        if (joystick.axis == Vector2.zero) {
            Debug.Log("eh");
            speed = Vector3.zero;
        }
    }

    private void HandleHead() {
        Vector3 oldPosition = cameraRig.position;
        Quaternion oldRotation = cameraRig.rotation;

        transform.eulerAngles = new Vector3(0.0f, head.rotation.eulerAngles.y, 0.0f);

        cameraRig.position = oldPosition;
        cameraRig.rotation = oldRotation;
    }

    public void Move(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta) {
        Vector3 orientationEuler = new Vector3(0, transform.eulerAngles.y, 0);
        Quaternion orientation = Quaternion.Euler(orientationEuler);
        Vector3 movement = Vector3.zero;
        
        speed += new Vector3(axis.x, 0, axis.y) * sensitivity;
        speed = new Vector3(Mathf.Clamp(speed.x, -maxSpeed, maxSpeed), 0 ,Mathf.Clamp(speed.z, -maxSpeed, maxSpeed));

        movement += orientation * speed * Time.deltaTime;

        characterController.Move(movement);
    }


    private void HandleHeight() {
        float headHeight = Mathf.Clamp(head.localPosition.y, 1, 2);
        characterController.height = headHeight;

        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y += characterController.skinWidth;

        newCenter.x = head.localPosition.x;
        newCenter.z = head.localPosition.z;

        newCenter = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * newCenter;

        characterController.center = newCenter;
    }
}
