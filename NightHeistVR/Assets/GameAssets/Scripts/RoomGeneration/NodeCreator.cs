using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCreator : MonoBehaviour
{
    public int sizeX, sizeY;
    public int pieceSize;
    public GameObject pieceNode;
    
    private PieceNode[,] placedPieces;

    private void Start() {
        placedPieces = new PieceNode[sizeX, sizeY];
        GenerateGrid();
    }

    private void GenerateGrid() {
        for (int y = 0; y < sizeY; y++) {
            for (int x = 0; x < sizeX; x++) {
                Vector3 pos = new Vector3(x * pieceSize, 0, y * pieceSize);
                PieceNode node = Instantiate(pieceNode, pos, Quaternion.identity, transform).GetComponent<PieceNode>();
                node.Create();

                placedPieces[x, y] = node;
            }
        }

        StartAlgorithm();
    }

    public void StartAlgorithm() {
        int randomX = Random.Range(0, sizeX);
        int randomY = Random.Range(0, sizeY);

        placedPieces[randomX, randomY].SetGround(this);
    }
}
