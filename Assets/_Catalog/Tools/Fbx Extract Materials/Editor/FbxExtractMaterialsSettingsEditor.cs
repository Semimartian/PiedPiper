using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FbxExtractMaterialsSettings))]
public class FbxExtractMaterialsSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FbxExtractMaterialsSettings settings = (FbxExtractMaterialsSettings)target;

        //base.OnInspectorGUI();
        EditorGUILayout.ToggleLeft("Active", settings.Active);

        EditorGUILayout.Space();
        if(GUILayout.Button("Choose Materials Folder"))
        {
            string path = EditorUtility.OpenFolderPanel("Choose Materials Folder", Application.dataPath, "");
            if (path.StartsWith(Application.dataPath))
            {
                path = "Assets" + path.Substring(Application.dataPath.Length);
            }
            settings.MaterialsFolder = path;
        }
        GUI.enabled = false;
        TextField("Folder", settings.MaterialsFolder);
        GUI.enabled = true;
    }

    private string TextField(string label, string text)
    {
        var textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x;
        return EditorGUILayout.TextField(label, text);
    }
}
