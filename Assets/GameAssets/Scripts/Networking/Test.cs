using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void Jump() {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 4, ForceMode.Impulse);
    }
}
