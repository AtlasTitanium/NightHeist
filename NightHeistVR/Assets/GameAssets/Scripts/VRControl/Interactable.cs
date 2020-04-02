using UnityEngine;
using System;

public class Interactable : MonoBehaviour
{
    public event Action use;

    public void Use() {
        use?.Invoke();
    }
}
