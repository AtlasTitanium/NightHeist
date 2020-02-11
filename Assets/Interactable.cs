using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    [HideInInspector]
    public Hand m_ActiveHand = null;

    public event Action pickup;
    public event Action use;
    public event Action drop;

    public void Pickup() {
        pickup.Invoke();
    }

    public void Use() {
        use.Invoke();
    }

    public void Drop() {
        drop.Invoke();
    }
}
