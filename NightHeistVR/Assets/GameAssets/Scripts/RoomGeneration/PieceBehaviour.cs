using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType { Ground, Wall, Corridor, Corner }
public class PieceBehaviour : MonoBehaviour
{
    public PieceType pieceType;
}
