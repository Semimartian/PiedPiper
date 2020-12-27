using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fbx Extract Materials Settings", fileName = "Fbx Extract Materials Settings")]
public class FbxExtractMaterialsSettings : ScriptableObject
{
    public bool Active;
    [Tooltip("If left empty materials will be created in the model's import folder /Materials/*.mat")]
    public string MaterialsFolder;
}
