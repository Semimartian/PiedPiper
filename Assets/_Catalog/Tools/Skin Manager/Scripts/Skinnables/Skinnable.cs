using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skinnable : MonoBehaviour
{
    public SkinnableType type;

    public static List<Skinnable> skinnables = new List<Skinnable>();

    [HideInInspector]
    public List<ObjectColorSave> saveColors;

    internal virtual void Awake()
    {
        skinnables.Add(this);
    }

    internal virtual void Setup()
    {

    }

    public virtual void ApplySkin()
    {

    }
}
