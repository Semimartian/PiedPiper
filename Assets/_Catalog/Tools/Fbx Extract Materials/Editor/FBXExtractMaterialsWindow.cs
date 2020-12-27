using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class FBXExtractMaterialsWindow : EditorWindow
{
    public bool Active;
    public string MaterialsPath;
    private bool _debug = false;

    [MenuItem("CrazyLabs/Tech Art Tools/FBX Extract Materials")]
    static void Init()
    {
        // Get existing open window or if none, make a new one
        FBXExtractMaterialsWindow window = (FBXExtractMaterialsWindow)GetWindow(typeof(FBXExtractMaterialsWindow));

        var settings            = window.GetOrCreateSettings();

        window.MaterialsPath    = settings.MaterialsFolder;
        window.Active           = settings.Active;

        window.Show();
    }

    void OnGUI()
    {
        var settings = GetOrCreateSettings();

        GUILayout.Label("Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        Active = EditorGUILayout.ToggleLeft("Active", Active);
        settings.Active = Active;
        EditorGUILayout.Space();

        if (GUILayout.Button("Choose Materials Folder"))
        {
            string materialsPath = EditorUtility.OpenFolderPanel("Choose Materials Folder", Application.dataPath, "");
            // get relative path
            if (materialsPath.StartsWith(Application.dataPath))
                materialsPath = "Assets" + materialsPath.Substring(Application.dataPath.Length);

            settings.MaterialsFolder = materialsPath;
        }

        // show grayed out folder path
        GUI.enabled = false;
        TextField("Folder", settings.MaterialsFolder);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // tool description
        GUILayout.TextArea( "This tool automaticly extracts materials to the selected folder when importing models\n" +
                            "each model will have its own folder with the same name as the model");
        GUI.enabled = true;
    }

    /// <summary>
    /// checks if a settings file exist and creates it if necessary
    /// </summary>
    /// <returns></returns>
    private FbxExtractMaterialsSettings GetOrCreateSettings()
    {
        var settings = Resources.Load<FbxExtractMaterialsSettings>("FBXExtractMaterialsConfig");
        if (settings == null)
        {
            settings = CreateInstance<FbxExtractMaterialsSettings>();

            string resourcesFolder = GetToolResourceFolder();

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(resourcesFolder + "FBXExtractMaterialsConfig.asset");

            AssetDatabase.CreateAsset(settings, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }

        return settings;
    }

    /// <summary>
    /// creates a text field with no space between the lable & text
    /// </summary>
    /// <param name="label"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    private string TextField(string label, string text)
    {
        var textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x;
        return EditorGUILayout.TextField(label, text);
    }

    /// <summary>
    /// finds the tool folder, checks if a resources folder exist and creates it if necessary
    /// </summary>
    /// <returns></returns>
    private string GetToolResourceFolder()
    {
        string scriptName = "FbxExtractMaterials.cs";
        string datapath = Application.dataPath.ToString();
        if(_debug) Debug.Log("data path: " + datapath);

        // get tool's script path
        string[] scripts = Directory.GetFiles(datapath, scriptName, SearchOption.AllDirectories);
        if (_debug) Debug.Log("script path: " + scripts[0]);

        // remove absolot path
        string relativePath = scripts[0].Remove(0, datapath.Length - 6);

        // subtruct the script file name
        int startIndex = relativePath.ToCharArray().Length - scriptName.ToCharArray().Length;
        relativePath = relativePath.Remove(startIndex, scriptName.ToCharArray().Length);

        if (_debug) Debug.Log("relative path: " + relativePath);

        string toolResourceFolderPath;
        if (Directory.Exists(relativePath + "Resources"))
        {
            // if exist get resources folder
            if (_debug) Debug.Log("resources exists");
            toolResourceFolderPath = relativePath + "Resources/";
        }
        else
        {
            // if dosent exist create resources folder in the tools editor folder
            if (_debug) Debug.Log("resources does not exists");
            startIndex = scripts[0].ToCharArray().Length - scriptName.ToCharArray().Length;

            string newfolderparent = scripts[0].Remove(startIndex, scriptName.ToCharArray().Length);
            if (_debug) Debug.Log("new folder parent path: " + newfolderparent);

            var di = Directory.CreateDirectory(newfolderparent + "Resources/");
            toolResourceFolderPath = di.FullName;
            AssetDatabase.Refresh();
            
            toolResourceFolderPath = toolResourceFolderPath.Remove(0, toolResourceFolderPath.Length - relativePath.Length - 10);
        }

        if (_debug) Debug.Log("Tool Resource Folder: " + toolResourceFolderPath);
        return toolResourceFolderPath;
    }
}
