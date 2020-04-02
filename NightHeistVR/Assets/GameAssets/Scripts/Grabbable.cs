using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
    [HideInInspector]
    public Hand m_ActiveHand = null;

    public event Action pickup;
    public event Action drop;

    public void Pickup() {
        pickup?.Invoke();
    }

    public void Drop() {
        drop?.Invoke();
    }
}
