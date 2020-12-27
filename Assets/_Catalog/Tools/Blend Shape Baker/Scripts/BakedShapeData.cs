using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CrazyLabs/Blend Shape Bake Data")]
public class BakedShapeData : ScriptableObject
{
    public int       framesCount;
    public Vector3[] minMaxPos;
    public Texture2D texture;
}
