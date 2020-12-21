using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skin Manager/Skin Manager Config")]
public class SkinManagerConfig : ScriptableObject
{
    [Header("Skinnable Types")]
    public SkinnableType types;

    [Header("Define Types")]
    public List<string> defineTypes;
}

