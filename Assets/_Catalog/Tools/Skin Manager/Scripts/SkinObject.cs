using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkinObject
{
    public SkinObject(SkinnableType type)
    {
        this.type = type;
        materials = null;
        materialColors    = null;
    }

    [HideInInspector]
    public SkinnableType     type;
    public GameObject        prefab;
    public Mesh              mesh;
    public Material[]        materials;
    public ObjectColor[]     materialColors;
    public LightObject       lightObject;
    public ParticlesColor[]  particlesColors;
}

[System.Serializable]
public struct LightObject
{
    public Color lightColor;
    public float intensity;
}

[System.Serializable]
public struct ObjectColor
{
    public int[]    materialIds;
    public Color    color;
    public string[] shaderProperties;
}

[System.Serializable]
public struct ObjectColorSave
{
    public int[] materialIds;
    public string shaderProperty;
}

[System.Serializable]
public struct ParticlesColor
{
    public Gradient         particleSystemGrad;
    public ParticleGradType gradType;
}

public enum ParticleGradType
{
    StartColor,
    OverLife,
    BySpeed,
}
