using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Skinnable), true)]
public class SkinnableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Skinnable skinnable = (Skinnable)target;

        base.OnInspectorGUI();

        GUILayout.Space(10);

        if(GUILayout.Button("Save Skinnable"))
        {
            SaveSkinnable(skinnable);
        }
    }

    public void SaveSkinnable(Skinnable skinnable)
    {
        bool replacePrefab        = false;
        bool changeMesh           = false;
        bool changeMaterials      = false;
        bool changeMatColors      = false;
        bool changeLightColor     = false;
        bool changeLightIntensity = false;

        bool changeParticleColors = false;
        bool applyToChildren      = false;

        if (skinnable is SkinnableGeneral)
        {
            replacePrefab        = false;
            changeMaterials      = (skinnable as SkinnableGeneral).changeMaterials;
            changeMatColors      = (skinnable as SkinnableGeneral).changeMaterialColor;
            changeParticleColors = false;
            applyToChildren      = false;

            if (skinnable is SkinnableMesh)
            {
                replacePrefab        = false;
                changeMesh           = (skinnable as SkinnableMesh).changeMesh;
                changeMaterials      = (skinnable as SkinnableGeneral).changeMaterials;
                changeMatColors      = (skinnable as SkinnableGeneral).changeMaterialColor;
                changeParticleColors = false;
                applyToChildren      = false;
            }
            else if (skinnable is SkinnablePrefab)
            {
                replacePrefab        = (skinnable as SkinnablePrefab).replacePrefab;
                changeMesh           = false;
                changeMaterials      = (skinnable as SkinnablePrefab).changeMaterials;
                changeMatColors      = (skinnable as SkinnablePrefab).changeMaterialColor;
                changeParticleColors = false;
                applyToChildren      = false;
            }
            else if (skinnable is SkinnableParticles)
            {
                replacePrefab        = false;
                changeMesh           = false;
                changeMaterials      = (skinnable as SkinnableParticles).changeMaterials;
                changeMatColors      = (skinnable as SkinnableParticles).changeMaterialColor;
                changeParticleColors = (skinnable as SkinnableParticles).changeParticleColors;
            }
        }
        if (skinnable is SkinnableLight)
        {
            changeLightColor     = (skinnable as SkinnableLight).changeLightColor;
            changeLightIntensity = (skinnable as SkinnableLight).changeLightIntensity;
        }

        SaveSkinnableWindow.Init(skinnable, replacePrefab, changeMesh, changeMaterials, changeMatColors, changeLightColor, changeLightIntensity, changeParticleColors, applyToChildren);
    }
}
