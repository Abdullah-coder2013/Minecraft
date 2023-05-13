using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeAttributes", menuName = "Minecraft/BiomeAttribute")]
public class BiomeAttributes : ScriptableObject
{

    [Header("Attributes")]
    public string biomeName;

    public int offset;

    public float scale;
    public int terrainHeight;
    public float terrainScale;
    public Lode[] lodes;

    [Header("Major Flora")]

    public int majorFloraIndex;

    public float majorFloraZoneScale = 1.3f;
    public bool placeMajorFlora = true;

    public byte surfaceBlock;
    public byte subSurfaceBlock;

    [Range(0.1f,1f)]
    public float majorFloraZoneThreshold = 0.6f;
    public float majorFloraPlacementScale = 10f;

    [Range(0.1f, 1f)]
    public float majorFloraPlacementThreshold = 0.8f;

    public int maxHeight = 5;
    public int minHeight = 3;
}
[System.Serializable]

public class Lode {
    public string nodeName;
    public byte blockID;
    public int minHeight;
    public int maxHeight;
    public float scale;
    public float threshold;
    public float noiseOffset;
}