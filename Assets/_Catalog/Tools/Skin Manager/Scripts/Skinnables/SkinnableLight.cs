using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkinnableLight : Skinnable
{
    public bool   changeLightColor;
    public bool   changeLightIntensity;

    private Light _light;

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

        _light = GetComponent<Light>();

        ApplySkin();
    }

    public override void ApplySkin()
    {
        _skinObject = _skinManager.skinLookup[type];

        if(changeLightColor)
            _light.color     = _skinObject.lightObject.lightColor;
        if (changeLightIntensity)
            _light.intensity = _skinObject.lightObject.intensity;
    }

    public LightObject GetLightObject()
    {
        if(_light == null)
            _light = GetComponent<Light>();

        LightObject lightObject = new LightObject();
        lightObject.lightColor  = _light.color;
        lightObject.intensity   = _light.intensity;

        return lightObject;
    }
    //public List<ObjectColor> GetColors(List<ObjectColorSave> objectColorSave)
    //{
    //    if (_renderer == null)
    //        _renderer = GetComponent<Renderer>();

    //    List<ObjectColor> objectColors = new List<ObjectColor>();

    //    Material[] materials;
    //    if (EditorApplication.isPlaying)
    //        materials = _renderer.materials;
    //    else
    //        materials = _renderer.sharedMaterials;

    //    for (int m = 0; m < _renderer.sharedMaterials.Length; m++)
    //    {
    //        for (int c = 0; c < objectColorSave.Count; c++) // material id check
    //        {
    //            bool idCheck = false;
    //            for (int id = 0; id < objectColorSave[c].materialIds.Length; id++)
    //            {
    //                if (objectColorSave[c].materialIds[id] == m)
    //                {
    //                    idCheck = true;
    //                    break;
    //                }
    //            }
    //            if (objectColorSave[c].materialIds.Length > 0 && !idCheck)
    //                continue;

    //            ObjectColor objColor = new ObjectColor();
    //            objColor.materialIds = objectColorSave[c].materialIds;
                

    //            if (objectColorSave[c].shaderProperty != "") // get property colors
    //            {
    //                objColor.color            = materials[m].GetColor(objectColorSave[c].shaderProperty);
    //                objColor.shaderProperties = new string[] { objectColorSave[c].shaderProperty };
    //            }
    //            else
    //            {
    //                objColor.color            = materials[m].color;
    //                objColor.shaderProperties = new string[0];
    //            }

    //            objectColors.Add(objColor);
    //        }
    //    }

    //    return objectColors;
    //}
}
