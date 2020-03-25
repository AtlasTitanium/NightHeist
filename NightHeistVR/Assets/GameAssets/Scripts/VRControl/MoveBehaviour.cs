using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MoveBehaviour : MonoBehaviour
{
    public SteamVR_Input_Sources inputHand;
    public SteamVR_Action_Vector2 joystick;

    public Transform head;
    public float moveSpeed;

    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        joystick.AddOnAxisListener(Move, inputHand);
    }

    public void Move(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta) {
        Vector3 moveDir = axis.y * head.forward + axis.x * head.right;
        rb.MovePosition(transform.position + moveDir.normalized * moveSpeed * Time.deltaTime);
    }
}
