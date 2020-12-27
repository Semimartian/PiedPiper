using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnableMesh : SkinnableGeneral
{
    public bool changeMesh;

    private MeshFilter _meshFilter;

    internal override void Awake()
    {
        base.Awake();
        Setup();
    }

    internal override void Setup()
    {

        _meshFilter = GetComponent<MeshFilter>();

        base.Setup();
    }

    public override void ApplySkin()
    {
        base.ApplySkin();
        if (changeMesh && _skinObject.mesh != null)
            _meshFilter.mesh = _skinObject.mesh;
    }

    public Mesh GetMesh()
    {
        if(_meshFilter == null)
            _meshFilter = GetComponent<MeshFilter>();

        return _meshFilter.sharedMesh;
    }
}
