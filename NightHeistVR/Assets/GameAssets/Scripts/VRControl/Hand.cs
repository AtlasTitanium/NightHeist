using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{
    public SteamVR_Action_Pose poseAction = SteamVR_Input.GetAction<SteamVR_Action_Pose>("Pose");
    public SteamVR_Action_Single grabAction;
    public SteamVR_Action_Boolean pinchAction;
    public SteamVR_Action_Boolean triggerAction;
    public SteamVR_Input_Sources inputSource;

    public Transform handForward;
    
    private FixedJoint joint = null;

    private Interactable currentInteractable = null;
    private List<Interactable> contactInteractables = new List<Interactable>();

    private Pinchable currentPinchable = null;
    private List<Pinchable> contactPinchables = new List<Pinchable>();

    private void Awake() {
        SteamVR.Initialize();
        joint = GetComponent<FixedJoint>();
    }
    
    void Start() {
        grabAction.AddOnChangeListener(Grab, inputSource);
        triggerAction.AddOnStateDownListener(TriggerDown, inputSource);
    }

    private void Update() {
        if (pinchAction.state) {
            Grab();
        } else if (currentPinchable != null) {
            currentPinchable = null;
        }
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        if (currentInteractable != null) {
            currentInteractable.Use();
        }
        Debug.Log("Trigger is down");
    }

    public void Grab(SteamVR_Action_Single fromAction, SteamVR_Input_Sources fromSource, float axis, float delta) {
        Debug.Log("grab");
        if(axis >= 0.05f && currentInteractable == null) {
            Pickup();
        } else if (axis <= 0.01f && currentInteractable != null) {
            Drop();
        }
    }

    //SetControllerPositions
    private void LateUpdate() {
        transform.localPosition = poseAction[inputSource].localPosition;
        transform.localRotation = poseAction[inputSource].localRotation;
    }

    public void Grab() {
        if(currentPinchable == null) {
            currentPinchable = GetNearestPinchable();
            return;
        }
        currentPinchable.Hold(this.transform);
    }

    public void Pickup() {
        currentInteractable = GetNearestInteractable();
        if (!currentInteractable) return;
        Debug.Log("grabbing");

        currentInteractable.transform.position = transform.position;
        currentInteractable.transform.rotation = handForward.rotation;
        Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
        joint.connectedBody = targetBody;

        currentInteractable.m_ActiveHand = this;
        currentInteractable.Pickup();
    }

    public void Drop() {
        if (!currentInteractable) return;
        Debug.Log("dropping");
        currentInteractable.Drop();

        Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = poseAction[inputSource].velocity;
        targetBody.angularVelocity = poseAction[inputSource].angularVelocity;

        joint.connectedBody = null;
        currentInteractable.m_ActiveHand = null;
        currentInteractable = null;
    }
    
    //Get close items
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Interactable>()) {
            if (!other.GetComponent<Interactable>().m_ActiveHand) {
                contactInteractables.Add(other.GetComponent<Interactable>());
            }
        } else if (other.GetComponent<Pinchable>()) {
            contactPinchables.Add(other.GetComponent<Pinchable>());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<Interactable>()) {
            if (contactInteractables.Contains(other.GetComponent<Interactable>())) {
                contactInteractables.Remove(other.GetComponent<Interactable>());
            }
        } else if (other.GetComponent<Pinchable>()) {
            if (contactPinchables.Contains(other.GetComponent<Pinchable>())) {
                contactPinchables.Remove(other.GetComponent<Pinchable>());
            }
        }
    }

    private Interactable GetNearestInteractable() {
        Interactable closest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach (Interactable i in contactInteractables) {
            distance = (i.transform.position - transform.position).sqrMagnitude;

            if(distance < minDistance) {
                minDistance = distance;
                closest = i;
            }
        }

        return closest;
    }

    private Pinchable GetNearestPinchable() {
        Pinchable closest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach (Pinchable i in contactPinchables) {
            distance = (i.transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance) {
                minDistance = distance;
                closest = i;
            }
        }

        return closest;
    }
}
