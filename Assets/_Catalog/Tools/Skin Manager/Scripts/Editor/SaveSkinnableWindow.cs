using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class SaveSkinnableWindow : EditorWindow
{
    private Skinnable _skinnable;

    private bool _savePrefab;
    private bool _saveMesh;
    private bool _saveMaterials;
    private bool _saveMatColors;

    private bool _saveLightColor;
    private bool _saveLightIntensity;

    private bool _saveParticleColors;
    private bool _saveChildren;

    private bool _saveParticleStartColor;
    private bool _saveParticleColorOverLife;
    private bool _saveParticleColorBySpeed;

    private int  _pickerFlag = 0;

    private SkinConfig _skin;

    public List<ObjectColorSave> saveColors;

    public static void Init(Skinnable skinnable, bool replacePrefab = false, bool changeMesh = false, bool changeMaterials = false, 
                            bool changeMatColors = false, bool changeLightColor = false, bool changeLightIntensity = false, bool changeParticleColors = false, bool applyToChildren = false)
    {
        SaveSkinnableWindow window = (SaveSkinnableWindow)GetWindow(typeof(SaveSkinnableWindow));

        window._skinnable          = skinnable;

        window._savePrefab         = replacePrefab;
        window._saveMesh           = changeMesh;
        window._saveMaterials      = changeMaterials;
        window._saveMatColors      = changeMatColors;

        window._saveLightColor     = changeLightColor;
        window._saveLightIntensity = changeLightIntensity;

        window._saveParticleColors = changeParticleColors;
        window._saveChildren       = applyToChildren;

        window.saveColors          = skinnable.saveColors;

        window.minSize = new Vector2(250, 250);
        window.Show();
    }

    private void OnGUI()
    {
        if (_skinnable is SkinnableGeneral)
        {
            if(_skinnable is SkinnableMesh)
            {
                GUILayout.Label("Save Mesh Skin", EditorStyles.boldLabel);
                _saveMesh = EditorGUILayout.Toggle("Save Mesh", _saveMesh);
            }
            else if(_skinnable is SkinnablePrefab)
            {
                GUILayout.Label("Save Prefab Skin", EditorStyles.boldLabel);
                _savePrefab = EditorGUILayout.Toggle("Save Prefab", _savePrefab);
                if(_savePrefab == true)
                {
                    if (EditorApplication.isPlaying)
                        EditorGUILayout.HelpBox("'Save Prefab' does not works in play mode and will be ignored", MessageType.Warning);
                    if ((_skinnable as SkinnablePrefab).objectToReplace == null)
                        EditorGUILayout.HelpBox("There is no prefab assigned to the 'Object To Replace' reference, please assign a prefab", MessageType.Warning);
                }
                
            }
            else if (_skinnable is SkinnableParticles)
            {
                GUILayout.Label("Save Particles Skin", EditorStyles.boldLabel);

                _saveParticleColors = EditorGUILayout.Toggle("Save Particles Colors", _saveParticleColors);

                if (_saveParticleColors)
                {
                    GUIStyle gs = new GUIStyle();
                    gs.padding.left = 15;
                    GUILayout.BeginVertical(gs);
                    _saveParticleStartColor    = EditorGUILayout.Toggle("Save Start Color"    , _saveParticleStartColor);
                    _saveParticleColorOverLife = EditorGUILayout.Toggle("Save Color Over Life", _saveParticleColorOverLife);
                    _saveParticleColorBySpeed  = EditorGUILayout.Toggle("Save Color By Speed" , _saveParticleColorBySpeed);
                    GUILayout.EndVertical();
                }
            }
            else
                GUILayout.Label("Save General Skin", EditorStyles.boldLabel);

            _saveMaterials = EditorGUILayout.Toggle("Save Materials Properties"      , _saveMaterials);
            _saveMatColors = EditorGUILayout.Toggle("Save Material Colors", _saveMatColors);
            
            if (_saveMatColors)
            {
                ScriptableObject target = this;
                SerializedObject so = new SerializedObject(target);
                SerializedProperty objectColorsProp = so.FindProperty("saveColors");

                EditorGUILayout.PropertyField(objectColorsProp, true);
                so.ApplyModifiedProperties();
                _skinnable.saveColors = saveColors;
            }
        }
        else if (_skinnable is SkinnableLight)
        {
            GUILayout.Label("Save Light Skin", EditorStyles.boldLabel);

            _saveLightColor     = EditorGUILayout.Toggle("Save Light Color", _saveLightColor);
            _saveLightIntensity = EditorGUILayout.Toggle("Save Light Intensity", _saveLightIntensity);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Choose Skin Asset"))
            EditorGUIUtility.ShowObjectPicker<SkinConfig>(null, true, "", _pickerFlag);

        if (Event.current.commandName == "ObjectSelectorUpdated")
            _skin = EditorGUIUtility.GetObjectPickerObject() as SkinConfig;

        GUILayout.Space(10);

        EditorGUI.BeginDisabledGroup(_skin == null);
        if (GUILayout.Button("Save"))
        {
            SaveSkinnable(_skin.skinObjects);
            Debug.Log("Skin Object Saved");
        }
        EditorGUI.EndDisabledGroup();
    }

    void OnOpenPicker(object obj, System.EventArgs e)
    {
        _pickerFlag = 1;
    }

    private void SaveSkinnable(SkinManagerSkinObjects skinObjects)
    {
        Dictionary<SkinnableType, SkinObject> lookup = skinObjects.GetLookup();

        if (_skinnable is SkinnableGeneral)
        {
            SkinnableGeneral skn = _skinnable as SkinnableGeneral;
            if (_skinnable is SkinnablePrefab)
                (_skinnable as SkinnablePrefab).AssignRendererRef();

            if (_saveMaterials)
            {
                SaveMaterials(lookup, skn);
            }
            if (_saveMatColors)
            {
                lookup[skn.type].materialColors = skn.GetColors(saveColors).ToArray();
            }
        }
        if (_skinnable is SkinnableMesh)
        {
            SkinnableMesh skn = _skinnable as SkinnableMesh;
            if (_saveMesh)
                lookup[skn.type].mesh = skn.GetMesh();
        }
        else if (_skinnable is SkinnablePrefab && !EditorApplication.isPlaying)
        {
            SkinnablePrefab skn = _skinnable as SkinnablePrefab;

            GameObject objectToReplace = (_skinnable as SkinnablePrefab).objectToReplace;
            if(objectToReplace != null)
                lookup[skn.type].prefab = PrefabUtility.GetCorrespondingObjectFromSource(objectToReplace);
        }
        else if (_skinnable is SkinnableParticles)
        {
            SkinnableParticles skn = _skinnable as SkinnableParticles;

            if (_saveParticleColors)
            {
                var colors = skn.GetParticleColors();
                ParticlesColor[] particlesColors = new ParticlesColor[colors.Count];

                int colorsCount = 0;
                if (_saveParticleStartColor)
                {
                    particlesColors[colorsCount] = colors[ParticleGradType.StartColor];
                    colorsCount++;
                }
                if (_saveParticleColorOverLife)
                {
                    particlesColors[colorsCount] = colors[ParticleGradType.OverLife];
                    colorsCount++;
                }
                if (_saveParticleColorBySpeed)
                {
                    particlesColors[colorsCount] = colors[ParticleGradType.BySpeed];
                }

                lookup[skn.type].particlesColors  = particlesColors;
            }
        }

        if(_skinnable is SkinnableLight)
        {
            SkinnableLight skn = _skinnable as SkinnableLight;

            lookup[skn.type].lightObject = skn.GetLightObject();
        }
    }

    private void SaveMaterials(Dictionary<SkinnableType, SkinObject> lookup, SkinnableGeneral skn)
    {
        bool showWarning = false;

        if (lookup[skn.type].materials.Length > 0)
        {
            Material[] mats = skn.GetMaterials();

            for (int i = 0; i < mats.Length; i++)
            {
                if (lookup[skn.type].materials[i] != null)
                    lookup[skn.type].materials[i].CopyPropertiesFromMaterial(mats[i]);
                else
                    showWarning = true;
            }
        }
        else
            showWarning = true;

        if (showWarning)
            Debug.LogWarning("could not save the object materials! " +
                "there is no material assigned in the skin asset for this type of object, please assign a material in the skin asset");
    }
}
