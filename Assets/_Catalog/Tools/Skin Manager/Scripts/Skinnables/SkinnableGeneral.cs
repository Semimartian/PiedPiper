using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkinnableGeneral : Skinnable
{
    public bool          changeMaterials;
    public bool          changeMaterialColor;

    internal Renderer    _renderer;

    internal SkinManager _skinManager;
    internal SkinObject  _skinObject;

    internal override void Awake()
    {
        base.Awake();
        Setup();
    }

    internal override void Setup()
    {
        _skinManager = SkinManager.Instance;

        _renderer = GetComponent<Renderer>();

        ApplySkin();
    }

    public override void ApplySkin()
    {
        _skinObject = _skinManager.skinLookup[type];

        ApplyMaterials();
        ApplyColors();
    }

    internal void ApplyColors()
    {
        if (changeMaterialColor && _skinObject.materialColors.Length > 0)
        {
            for (int m = 0; m < _renderer.materials.Length; m++) // materials in renderer
            {
                for (int c = 0; c < _skinObject.materialColors.Length; c++) // colors in skin
                {
                    bool idCheck = false;
                    for (int i = 0; i < _skinObject.materialColors[c].materialIds.Length; i++) // material id check
                    {
                        if (_skinObject.materialColors[c].materialIds[i] == m)
                        {
                            idCheck = true;
                            break;
                        }
                    }
                    if (_skinObject.materialColors[c].materialIds.Length > 0 && !idCheck)
                        continue;

                    if (_skinObject.materialColors[c].shaderProperties.Length > 0) // material properties
                    {
                        for (int p = 0; p < _skinObject.materialColors[c].shaderProperties.Length; p++)
                        {
                            _renderer.materials[m].SetColor(_skinObject.materialColors[c].shaderProperties[p], _skinObject.materialColors[c].color);
                        }
                    }
                    else
                    {
                        _renderer.materials[m].color = _skinObject.materialColors[c].color;
                    }
                }
            }
        }
    }

    internal void ApplyMaterials()
    {
        if (changeMaterials && _skinObject.materials.Length > 0)
        {
            _renderer.materials = _skinObject.materials;
        }
    }

    public virtual Material[] GetMaterials()
    {
        if(_renderer == null)
            _renderer = GetComponent<Renderer>();
        
        if (EditorApplication.isPlaying)
            return _renderer.materials;
        else
            return _renderer.sharedMaterials;
    }

    public List<ObjectColor> GetColors(List<ObjectColorSave> objectColorSave)
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();

        List<ObjectColor> objectColors = new List<ObjectColor>();

        Material[] materials;
        if (EditorApplication.isPlaying)
            materials = _renderer.materials;
        else
            materials = _renderer.sharedMaterials;

        for (int m = 0; m < _renderer.sharedMaterials.Length; m++)
        {
            for (int c = 0; c < objectColorSave.Count; c++) // material id check
            {
                bool idCheck = false;
                for (int id = 0; id < objectColorSave[c].materialIds.Length; id++)
                {
                    if (objectColorSave[c].materialIds[id] == m)
                    {
                        idCheck = true;
                        break;
                    }
                }
                if (objectColorSave[c].materialIds.Length > 0 && !idCheck)
                    continue;

                ObjectColor objColor = new ObjectColor();
                objColor.materialIds = objectColorSave[c].materialIds;
                

                if (objectColorSave[c].shaderProperty != "") // get property colors
                {
                    objColor.color            = materials[m].GetColor(objectColorSave[c].shaderProperty);
                    objColor.shaderProperties = new string[] { objectColorSave[c].shaderProperty };
                }
                else
                {
                    objColor.color            = materials[m].color;
                    objColor.shaderProperties = new string[0];
                }

                objectColors.Add(objColor);
            }
        }

        return objectColors;
    }
}
