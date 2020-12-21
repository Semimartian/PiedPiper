using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public int          currentSkin;
    public SkinConfig[] skins;

    public Dictionary<SkinnableType, SkinObject> skinLookup;

    public static SkinManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogWarning("More then one SkinManager was created");
            Destroy(gameObject);
        }

        skinLookup = skins[currentSkin].skinObjects.GetLookup();
    }

    public void UpdateSkin()
    {
        skinLookup = skins[currentSkin].skinObjects.GetLookup();
        foreach (var skinnable in Skinnable.skinnables)
        {
            skinnable.ApplySkin();
        }
    }
}
