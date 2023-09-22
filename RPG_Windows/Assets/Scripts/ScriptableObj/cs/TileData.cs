using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Parameter Storage/Tile")]
public class TileData : ScriptableObject
{
    public Tile[] tiles;

    public float height;
}
