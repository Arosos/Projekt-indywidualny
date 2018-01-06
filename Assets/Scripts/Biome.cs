using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Biome
{
    [Range(0, 1)]
    public float threshold;
    public GameObject tilePrefab;
}
