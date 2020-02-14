using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{
    public SteamVR_Action_Boolean grabAction = null;

    private SteamVR_Behaviour_Pose poseBehaviour = null;
    private FixedJoint joint = null;

    private Interactable currentInteractable = null;
    private List<Interactable> contactInteractables = new List<Interactable>();

    private void Awake() {
        poseBehaviour = GetComponent<SteamVR_Behaviour_Pose>();
        joint = GetComponent<FixedJoint>();
    }

    private void Update() {
        if (grabAction.GetStateDown(poseBehaviour.inputSource)) {
            Pickup();
        }
        if (grabAction.GetStateUp(poseBehaviour.inputSource)) {
            Drop();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.GetComponent<Interactable>())
            return;

        contactInteractables.Add(other.GetComponent<Interactable>());
    }

    private void OnTriggerExit(Collider other) {
        if (!other.GetComponent<Interactable>())
            return;

        contactInteractables.Remove(other.GetComponent<Interactable>());
    }

    public void Pickup() {
        currentInteractable = GetNearestInteractable();

        if (!currentInteractable) return;
        if (currentInteractable.m_ActiveHand) currentInteractable.m_ActiveHand.Drop();

        currentInteractable.transform.position = transform.position;
        Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
        joint.connectedBody = targetBody;

        currentInteractable.m_ActiveHand = this;
        currentInteractable.Pickup();
    }

    public void Drop() {
        if (!currentInteractable) return;
        currentInteractable.Drop();

        Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = poseBehaviour.GetVelocity();
        targetBody.angularVelocity = poseBehaviour.GetAngularVelocity();

        joint.connectedBody = null;
        currentInteractable.m_ActiveHand = null;
        currentInteractable = null;
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
}
