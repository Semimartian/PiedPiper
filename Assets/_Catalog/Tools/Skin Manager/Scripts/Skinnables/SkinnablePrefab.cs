using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnablePrefab : SkinnableGeneral
{
    public bool          replacePrefab;
    [Tooltip("if this remain empty the new Object will just be created as a child")]
    public GameObject    objectToReplace;

    internal override void Awake()
    {
        base.Awake();
        Setup();
    }

    internal override void Setup()
    {
        base.Setup();
        AssignRendererRef();
    }

    public override void ApplySkin()
    {
        _skinObject = _skinManager.skinLookup[type];

        if (replacePrefab)
        {
            if (objectToReplace != null)
                Destroy(objectToReplace);

            if (_skinObject.prefab != null)
            {
                objectToReplace = Instantiate(_skinObject.prefab, transform);
                _renderer       = objectToReplace.GetComponent<Renderer>();
            }
        }

        ApplyMaterials();
        ApplyColors();
    }

    public void AssignRendererRef()
    {
        if (objectToReplace != null)
            _renderer = objectToReplace.GetComponent<Renderer>();
    }
}
