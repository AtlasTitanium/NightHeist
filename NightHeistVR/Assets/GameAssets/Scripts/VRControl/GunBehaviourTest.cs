using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviourTest : MonoBehaviour
{
    public GameObject bullets;
    private Interactable interactable;

    private void Start() {
        interactable = GetComponent<Interactable>();
        interactable.use += Shoot;
    }

    private void Shoot() {
        GameObject currentBullet = Instantiate(bullets, transform.position, Quaternion.identity, transform);
        currentBullet.transform.rotation = transform.rotation;
        currentBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.Impulse);
        Destroy(currentBullet, 1);
    }
}
