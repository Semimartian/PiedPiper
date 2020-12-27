using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

[ExecuteInEditMode]
public class MaterialExtractorWindow : EditorWindow
{
    private string _selectedObjectsFolder = "";
    private string[] _selectedObjects;
    private string _destinationDirectory = "";

    [MenuItem("CrazyLabs/Tech Art Tools/Material Extractor")]
    static void Initialize()
    {
        MaterialExtractorWindow window = (MaterialExtractorWindow)GetWindow(typeof(MaterialExtractorWindow));

        //window._selectedObject = Selection.activeObject as GameObject;

        window.Show();
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Choose Materials Destination Directory"))
        {
            _destinationDirectory = EditorUtility.OpenFolderPanel("Choose Materials Directory", "", "");
        }
        if(_destinationDirectory == "")
            EditorGUILayout.HelpBox("Please Choose A Directory", MessageType.Warning);
        else
            EditorGUILayout.HelpBox(_destinationDirectory, MessageType.None);

        if(GUILayout.Button("Select Models Directory"))
        {
            _selectedObjectsFolder = EditorUtility.OpenFolderPanel("Choose Models Directory", "", "");
            _selectedObjects = Directory.GetFiles(_selectedObjectsFolder);
        }
        if (_selectedObjectsFolder == "")
            EditorGUILayout.HelpBox("Please Choose A Directory", MessageType.Warning);
        else
            EditorGUILayout.HelpBox(_selectedObjectsFolder, MessageType.None);


        EditorGUI.BeginDisabledGroup(_selectedObjectsFolder != "" && _destinationDirectory != "");
        if (GUILayout.Button("Extract Materials"))
        {
            for (int i = 0; i < _selectedObjects.Length; i++)   
            {
                ExtractMaterials(_selectedObjects[i], _destinationDirectory);
            }
        }
        EditorGUI.EndDisabledGroup();
    }

    public void ExtractMaterials(string assetPath, string destinationPath)
    {
        string relativeAssetPath = ConvertToRelativePath(assetPath);
        string relativeDestinationPath = ConvertToRelativePath(destinationPath);

        HashSet<string> hashSet = new HashSet<string>();
        IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(relativeAssetPath)
                                         where x.GetType() == typeof(Material)
                                         select x;
        foreach (Object item in enumerable)
        {
            string fileName = Path.GetFileNameWithoutExtension(relativeAssetPath);
            string newDirectory = Directory.CreateDirectory(destinationPath + "/" + fileName).Name;
            

            string path = Path.Combine(relativeDestinationPath + "/" + newDirectory, item.name) + ".mat";
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            string value = AssetDatabase.ExtractAsset(item, path);
            if (string.IsNullOrEmpty(value))
            {
                hashSet.Add(relativeAssetPath);
            }
        }

        foreach (string item2 in hashSet)
        {
            AssetDatabase.WriteImportSettingsIfDirty(item2);
            AssetDatabase.ImportAsset(item2, ImportAssetOptions.ForceUpdate);
        }
    }

    private string ConvertToRelativePath(string path)
    {
        string newPath = path;
        if (path.StartsWith(Application.dataPath))
        {
            newPath = "Assets" + path.Substring(Application.dataPath.Length);
        }
        return newPath;
    }

    public static T[] GetAtPath<T>(string path)
    {

        ArrayList al = new ArrayList();
        string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
        foreach (string fileName in fileEntries)
        {
            int index = fileName.LastIndexOf("/");
            string localPath = "Assets/" + path;

            if (index > 0)
                localPath += fileName.Substring(index);

            Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

            if (t != null)
                al.Add(t);
        }
        T[] result = new T[al.Count];
        for (int i = 0; i < al.Count; i++)
            result[i] = (T)al[i];

        return result;
    }
}
