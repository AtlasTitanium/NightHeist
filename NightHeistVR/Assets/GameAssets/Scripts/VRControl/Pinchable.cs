using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pinchable : MonoBehaviour
{
    public event Action<Transform> hold;

    public void Hold(Transform holderTransform) {
        hold?.Invoke(holderTransform);
    }
}
