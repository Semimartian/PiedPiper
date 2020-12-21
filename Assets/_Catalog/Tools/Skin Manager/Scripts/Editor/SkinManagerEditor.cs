using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkinManager))]
public class SkinManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SkinManager skinManager = (SkinManager)target;

        base.OnInspectorGUI();


        GUILayout.Space(20);
        if(GUILayout.Button("Change Skin"))
        {
            skinManager.UpdateSkin();
        }
    }
}
