using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KnobBehaviour : MonoBehaviour
{
    public Transform holdPosition;
    public float turnPercent;
    public Text dotShow;

    private float value;
    private float angleChanged;
    private Pinchable pinchable;

    private void Awake() {
        pinchable = GetComponent<Pinchable>();
        pinchable.hold += Holding;
    }

    private void Holding(Transform holder) {
        Debug.Log("knob turnin");
        Vector3 toOther = holdPosition.position - holder.position;
        float dot = Vector3.Dot(-holdPosition.right, toOther);
        dotShow.text = Math.Round(dot,2) + "";

        if ((dot >= 0.01f && value <= turnPercent) || (dot <= -0.01f && value >= 0)) {
            Vector3 currentRotation = transform.localRotation.eulerAngles;

            transform.Rotate(Vector3.up, dot * 200);

            angleChanged = transform.localRotation.eulerAngles.y - currentRotation.y;
            if (angleChanged >= turnPercent) {
                angleChanged -= 360;
            }
            else if (angleChanged <= -turnPercent) {
                angleChanged += 360;
            }

            Debug.Log(value);
            value = value + (angleChanged / 3.6f);
        }
    }
}
