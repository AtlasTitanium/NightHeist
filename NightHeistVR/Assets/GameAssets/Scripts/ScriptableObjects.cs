using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "MuseumHeist/ItemData", order = 1)]
public class ItemObjectData : ScriptableObject {
    public string title;
    public string publisher;

    [Range(0.01f, 1000000.00f)]
    public float salePrice;

    public Mesh objectMesh;
    public Material objectMat;
}
