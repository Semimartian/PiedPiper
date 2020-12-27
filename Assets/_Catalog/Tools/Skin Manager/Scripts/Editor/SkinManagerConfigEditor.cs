using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(SkinManagerConfig))]
public class SkinManagerConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SkinManagerConfig script = (SkinManagerConfig)target;

        base.OnInspectorGUI();
        EditorGUILayout.Space();

        if (GUILayout.Button("Save Types"))
        {
            SaveEnums(script.defineTypes);
            SaveSkinObjects(script.defineTypes);
        }
    }

    public static void SaveEnums(List<string> enums)
    {
        string enumName = "SkinnableType";
        string[] enumEntries = enums.ToArray();

        string[] guids = AssetDatabase.FindAssets("SkinManagerEnums");
        string path = "";
        for (int i = 0; i < guids.Length; i++)
        {
            path = AssetDatabase.GUIDToAssetPath(guids[i]);
            if (path.Contains("SkinManagerEnums.cs"))
            {
                break;
            }
            else if(i >= guids.Length)
            {
                Debug.LogWarning("Could not find SkinManagerEnums.cs");
                return;
            }
        }

        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            streamWriter.WriteLine("public enum " + enumName);
            streamWriter.WriteLine("{");
            for (int i = 0; i < enumEntries.Length; i++)
            {
                streamWriter.WriteLine("\t" + enumEntries[i] + ",");
            }
            streamWriter.WriteLine("}");
        }
        AssetDatabase.Refresh();
    }

    public static void SaveSkinObjects(List<string> enums)
    {
        string className = "SkinManagerSkinObjects";
        string[] enumEntries = enums.ToArray();

        string[] guids = AssetDatabase.FindAssets("SkinManagerSkinObjects");
        string path = "";
        for (int i = 0; i < guids.Length; i++)
        {
            path = AssetDatabase.GUIDToAssetPath(guids[i]);
            if (path.Contains("SkinManagerSkinObjects.cs"))
            {
                break;
            }
            else if (i >= guids.Length)
            {
                Debug.LogWarning("Could not find SkinManagerSkinObjects.cs");
                return;
            }
        }

        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            
            streamWriter.WriteLine("using UnityEngine;");
            streamWriter.WriteLine("using System.Collections.Generic;");
            streamWriter.WriteLine("");

            streamWriter.WriteLine("[System.Serializable]");
            streamWriter.WriteLine("public class " + className);
            streamWriter.WriteLine("{");

            // create members
            for (int i = 0; i < enumEntries.Length; i++)
            {
                streamWriter.WriteLine("\t" + "public SkinObject " + enumEntries[i] + " = new SkinObject(SkinnableType." + enumEntries[i] + ");");
            }

            // create lookup
            streamWriter.WriteLine("" + "\n");
            streamWriter.WriteLine("\t" + "public Dictionary<SkinnableType, SkinObject> GetLookup()");
            streamWriter.WriteLine("\t" + "{");
            streamWriter.WriteLine("\t\t" + "Dictionary<SkinnableType, SkinObject> skinLookup = new Dictionary<SkinnableType, SkinObject>()");
            streamWriter.WriteLine("\t\t" + "{");
            for (int i = 0; i < enumEntries.Length; i++)
            {
                streamWriter.WriteLine("\t\t\t" + "{ SkinnableType." + enumEntries[i] + ", " + enumEntries[i] +" },");
            }
            streamWriter.WriteLine("\t\t" + "};");
            streamWriter.WriteLine("");
            streamWriter.WriteLine("\t\t" + "return skinLookup;");
            streamWriter.WriteLine("\t" + "}");

            streamWriter.WriteLine("}");
        }
        AssetDatabase.Refresh();
    }
}
