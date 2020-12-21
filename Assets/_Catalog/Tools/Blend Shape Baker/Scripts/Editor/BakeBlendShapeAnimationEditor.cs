using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BakeBlendShapeAnimation))]
public class BakeBlendShapeAnimationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BakeBlendShapeAnimation script = (BakeBlendShapeAnimation)target;
        base.OnInspectorGUI();

        GUILayout.Space(10);

        if (GUILayout.Button("Choose Save Path"))
        {
            script.path = GetSavePath();
            Debug.Log("Save path: " + script.path);
        }
        
        GUILayout.Space(10);

        if (GUILayout.Button("Bake Frames"))
        {
            script.BakeFrames();
        }
    }

    public string GetSavePath()
    {
        return EditorUtility.OpenFolderPanel("Choose save folder", "", "");
    }
}
