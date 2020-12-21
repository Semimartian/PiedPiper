using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

[ExecuteInEditMode]
public class DuplicatorWindow : EditorWindow
{
    private static GameObject _selectedObject;
    private static Transform  _parent;

    private static Vector3    _selectedPos;
    private static Vector3    _selectedRot;
    private static Vector3    _parentPos;

    private static Vector3Int _duplicatesCount = new Vector3Int(1,1,1);
    private static Vector3    _stepSize;
    private static Vector3    _range;

    private static List<Transform> _transforms;

    static AnimBool _byStepAnim;
    static AnimBool _byRange;

    static bool _useLocal = false;

    [MenuItem("GameObject/CrazyLabs/Open Duplicator", false, 20)]
    [MenuItem("CrazyLabs/Tech Art Tools/Open Duplicator")]
    static void Initialize()
    {
        DuplicatorWindow window = (DuplicatorWindow)GetWindow(typeof(DuplicatorWindow));

        var temp = (GameObject)Selection.activeObject;
        if (temp != null)
            _selectedObject = temp;
        //else
        //    EditorGUILayout.HelpBox("Please Choose a ");

        _transforms = new List<Transform>();
        _transforms.Clear();

        window.Show();
    }

    void OnEnable()
    {
        _byStepAnim = new AnimBool(true);
        _byStepAnim.valueChanged.AddListener(Repaint);
        _byRange    = new AnimBool(false);
        _byRange   .valueChanged.AddListener(Repaint);
    }

    private void OnGUI()
    {
        if (_transforms == null)
            Initialize();

        EditorGUI.BeginChangeCheck();

        #region References
        EditorGUILayout.LabelField("Objects");
        _selectedObject  = EditorGUILayout.ObjectField    ("Selected Object", _selectedObject, typeof(GameObject), true) as GameObject;
        _parent          = EditorGUILayout.ObjectField    ("Designated Parent", _parent, typeof(Transform), true) as Transform;
        #endregion

        #region Settings
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Settings");

        _useLocal = EditorGUILayout.ToggleLeft("Local To Object", _useLocal);

        EditorGUILayout.BeginHorizontal();
        _byStepAnim.target = EditorGUILayout.ToggleLeft("By Step", _byStepAnim.target);
        _byRange.target    = !_byStepAnim.target;

        _byRange.target    = EditorGUILayout.ToggleLeft("By Range", _byRange.target);
        _byStepAnim.target = !_byRange.target;
        EditorGUILayout.EndHorizontal();
        #endregion

        #region Settings Fade Groups
        if (EditorGUILayout.BeginFadeGroup(_byStepAnim.faded)) // by step
        {
            _duplicatesCount = EditorGUILayout.Vector3IntField("Count", _duplicatesCount);
            _stepSize        = EditorGUILayout.Vector3Field("Step Size", _stepSize);

            if (_selectedObject != null 
            && (GUI.changed || _selectedPos != _selectedObject.transform.position || _selectedRot != _selectedObject.transform.eulerAngles || (_parent != null && _parentPos != _parent.position)))
            {
                UpdateTransform();
                LimitCountMin();

                DeleteDuplicated();
                DuplicateObjectsByStep(_stepSize);
            }
        }
        EditorGUILayout.EndFadeGroup();

        if (EditorGUILayout.BeginFadeGroup(_byRange.faded)) // by range
        {
            _duplicatesCount = EditorGUILayout.Vector3IntField("Count", _duplicatesCount);
            _range           = EditorGUILayout.Vector3Field("Range", _range);

            if (_selectedObject != null && GUI.changed
            && (GUI.changed || _selectedPos != _selectedObject.transform.position || _selectedRot != _selectedObject.transform.eulerAngles || (_parent != null && _parentPos != _parent.position)))
            {
                UpdateTransform();
                LimitCountMin();

                DeleteDuplicated();
                DuplicateObjectsByStep(GetStepSizeFromRange());
            }
        }
        EditorGUILayout.EndFadeGroup();
        #endregion

        EditorGUI.EndChangeCheck();

        #region Buttons
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Update/Duplicate"))
        {
            if (_selectedObject != null)
            {
                if (_byStepAnim.target)
                    DuplicateObjectsByStep(_stepSize);
                else
                    DuplicateObjectsByStep(GetStepSizeFromRange());
            }
            else
                Debug.LogWarning("Please Select an onject in the 'Selected Object' fiels");
        }
        EditorGUI.BeginDisabledGroup(_transforms.Count == 0);
        if (GUILayout.Button("Delete Duplicates"))
        {
            DeleteDuplicated();
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();
        #endregion
    }

    private void UpdateTransform()
    {
        _selectedPos = _selectedObject.transform.position;
        _selectedRot = _selectedObject.transform.eulerAngles;

        if (_parent != null)
            _parentPos   = _parent.position;
    }

    private void DuplicateObjectsByStep(Vector3 stepSize)
    {
        if (_transforms.Count > 0)
            DeleteDuplicated();
        else
            _transforms.Clear();

        Vector3 pos = _selectedObject.transform.position;
        for (int x = 0; x < _duplicatesCount.x; x++)
        {
            CreateObject(new Vector3(stepSize.x * x, 0, 0));

            for (int y = 0; y < _duplicatesCount.y; y++)
            {
                if (y % _duplicatesCount.y != 0)
                    CreateObject(new Vector3(stepSize.x * x, stepSize.y * y, 0));

                for (int z = 0; z < _duplicatesCount.z; z++)
                {
                    if (z % _duplicatesCount.z == 0) continue;
                    CreateObject(new Vector3(stepSize.x * x, stepSize.y * y, stepSize.z * z));
                }
            }
        }
    }

    private Vector3 GetStepSizeFromRange()
    {
        float xStep = (_duplicatesCount.x - 1) == 0 ? 0 : _range.x / (_duplicatesCount.x - 1);
        float yStep = (_duplicatesCount.y - 1) == 0 ? 0 : _range.y / (_duplicatesCount.y - 1);
        float zStep = (_duplicatesCount.z - 1) == 0 ? 0 : _range.z / (_duplicatesCount.z - 1);

        return new Vector3(xStep, yStep, zStep);
    }

    private void DeleteDuplicated()
    {
        for (int i = 0; i < _transforms.Count; i++)
        {
            DestroyImmediate(_transforms[i].gameObject);
        }
        _transforms.Clear();
    }

    private Transform CreateObject(Vector3 position)
    {
        Transform trans = Instantiate(_selectedObject, _parent).transform;

        if (_useLocal)
            trans.position = _selectedObject.transform.TransformPoint(position);
        else
            trans.position += position;

        _transforms.Add(trans);

        return trans;
    }

    private void LimitCountMin()
    {
        _duplicatesCount.x = _duplicatesCount.x < 1 ? 1 : _duplicatesCount.x;
        _duplicatesCount.y = _duplicatesCount.y < 1 ? 1 : _duplicatesCount.y;
        _duplicatesCount.z = _duplicatesCount.z < 1 ? 1 : _duplicatesCount.z;
    }
}
