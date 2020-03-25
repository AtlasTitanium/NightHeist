using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollision : MonoBehaviour
{
    public Transform head;
    public Transform centre;

    private void Update() {
        gameObject.transform.position = new Vector3(head.position.x, centre.position.y, head.position.z);
    }
}
