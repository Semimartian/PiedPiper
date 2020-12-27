//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomPropertyDrawer(typeof(SkinObject))]
//public class SkinObjectPropertyDrawer : PropertyDrawer
//{
//    private int _lineHeight = 20;

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        // Using BeginProperty / EndProperty on the parent property means that
//        // prefab override logic works on the entire property.
//        EditorGUI.BeginProperty(position, label, property);

//        // Draw label
//        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

//        // Don't make child fields be indented
//        var indent = EditorGUI.indentLevel;
//        EditorGUI.indentLevel = -8;

//        // Calculate rects
//        var prefabLblRct      = new Rect(position.x      , position.y + _lineHeight    , 60, _lineHeight);
//        var changePrefabRct   = new Rect(position.x + 90 , position.y + _lineHeight    , 20, _lineHeight);
//        var prefabRct         = new Rect(position.x + 110, position.y + _lineHeight    , 60, _lineHeight);
                              
//        var meshLblRct        = new Rect(position.x      , position.y + _lineHeight * 2, 60, _lineHeight);
//        var changeMeshRct     = new Rect(position.x + 90 , position.y + _lineHeight * 2, 20, _lineHeight);
//        var meshRct           = new Rect(position.x + 110, position.y + _lineHeight * 2, 60, _lineHeight);

//        var matLblRct         = new Rect(position.x      , position.y + _lineHeight * 3, 60, _lineHeight);
//        var changeMatRct      = new Rect(position.x + 90 , position.y + _lineHeight * 3, 20, _lineHeight);
//        var matRct            = new Rect(position.x + 110, position.y + _lineHeight * 3, 100, _lineHeight * 3);

//        var matColorLblRct    = new Rect(position.x      , position.y + _lineHeight * 4, 60, _lineHeight);
//        var changeMatColorRct = new Rect(position.x + 90 , position.y + _lineHeight * 4, 20, _lineHeight);
//        var matColorRct       = new Rect(position.x + 110, position.y + _lineHeight * 4, 100, _lineHeight * 3);

//        // Draw lables
//        EditorGUI.LabelField(prefabLblRct, "Prefab");
//        EditorGUI.LabelField(meshLblRct, "Mesh");
//        EditorGUI.LabelField(matLblRct, "Material");
//        EditorGUI.LabelField(matColorLblRct, "Material Color");

//        // Get bool values
//        var changePrefab   = property.FindPropertyRelative("changePrefab");
//        var changeMesh     = property.FindPropertyRelative("changeMesh");
//        var changeMat      = property.FindPropertyRelative("changeMat");
//        var changeMatColor = property.FindPropertyRelative("changeMatColor");


//        // Draw fields - passs GUIContent.none to each so they are drawn without labels
//        EditorGUI.PropertyField(changePrefabRct, property.FindPropertyRelative("changePrefab"), GUIContent.none);
//        if (changePrefab.boolValue)
//            EditorGUI.PropertyField(prefabRct, property.FindPropertyRelative("prefab"), GUIContent.none);

//        EditorGUI.PropertyField(changeMeshRct, property.FindPropertyRelative("changeMesh"), GUIContent.none);
//        if (changeMesh.boolValue)
//            EditorGUI.PropertyField(meshRct, property.FindPropertyRelative("mesh"), GUIContent.none);

//        EditorGUI.PropertyField(changeMatRct, property.FindPropertyRelative("changeMat"), GUIContent.none);
//        if (changeMat.boolValue)
//            EditorGUI.PropertyField(matRct, property.FindPropertyRelative("materials"), GUIContent.none);

//        EditorGUI.PropertyField(changeMatColorRct, property.FindPropertyRelative("changeMatColor"), GUIContent.none);
//        if (changeMatColor.boolValue)
//            EditorGUI.PropertyField(matColorRct, property.FindPropertyRelative("colors"), new GUIContent("colors"));

//        // Set indent back to what it was
//        EditorGUI.indentLevel = indent;
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return base.GetPropertyHeight(property, label) * 8;
//    }
//}
