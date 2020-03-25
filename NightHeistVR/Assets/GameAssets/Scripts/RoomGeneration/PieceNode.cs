using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceNode : MonoBehaviour
{
    public GameObject[] pieces;

    public void Create() {
        for (int i = 0; i < pieces.Length; i++) {
            pieces[i] = Instantiate(pieces[i], transform.position, Quaternion.identity, transform);
            pieces[i].SetActive(false);
        }
    }

    public void SetGround(NodeCreator creator) {
        foreach(GameObject piece in pieces) {
            piece.SetActive(true);
            if(piece.GetComponent<PieceBehaviour>().pieceType != PieceType.Ground) {
                Destroy(piece);
            }
        }
    }
}
