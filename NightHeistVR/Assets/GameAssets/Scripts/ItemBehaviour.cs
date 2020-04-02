using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grabbable))]
public class ItemBehaviour : MonoBehaviour
{
    [SerializeField]
    private ItemObjectData itemData;
    [SerializeField]
    private GameObject artPiece;
    [SerializeField]
    private GameObject spaceCube;
    
    private Grabbable grabBehaviour;
    private Collider artCollider;

    private Vector3 artpieceSize;
    private Vector3 spacecubeSize;
    private Vector3 colliderSize;
    private float lerpNumber;

    private void Start() {
        grabBehaviour = GetComponent<Grabbable>();
        artCollider = GetComponent<Collider>();

        artpieceSize = artPiece.transform.localScale;
        spacecubeSize = spaceCube.transform.localScale;
        colliderSize = artCollider.GetComponent<BoxCollider>().size;

        spaceCube.transform.localScale = Vector3.zero;

        grabBehaviour.pickup += PickupArt;
        grabBehaviour.drop += DropArt;
    }

    public void PickupArt() {
        StartCoroutine(GrabSwitch());
    }

    public void DropArt() {
        StartCoroutine(DropSwitch());
    }

    IEnumerator GrabSwitch() {
        for (int i = 0; i <= 10; i++) {
            lerpNumber = i / 10.0f;
            artPiece.transform.localScale = Vector3.Lerp(artpieceSize, spacecubeSize /2, lerpNumber);
            spaceCube.transform.localScale = Vector3.Lerp(Vector3.zero, spacecubeSize, lerpNumber);
            artCollider.GetComponent<BoxCollider>().size = Vector3.Lerp(colliderSize, Vector3.zero, lerpNumber);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator DropSwitch() {
        yield return new WaitForSeconds(2);
        for (int i = 10; i >= 0; i--) {
            lerpNumber = i / 10.0f;
            artPiece.transform.localScale = Vector3.Lerp(artpieceSize, spacecubeSize /2, lerpNumber);
            spaceCube.transform.localScale = Vector3.Lerp(Vector3.zero, spacecubeSize, lerpNumber);
            artCollider.GetComponent<BoxCollider>().size = Vector3.Lerp(colliderSize, Vector3.zero, lerpNumber);
            yield return new WaitForEndOfFrame();
        }
        
    }
}
