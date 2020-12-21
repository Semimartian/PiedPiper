using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackBakedBlendAnimation : MonoBehaviour
{
    public BakedShapeData bakedShapeData;
    [Range(0f, 1f)]
    public float animationTime;

    private int             _frames;
    private int             _currentFrame = 0;
    private float           _lerp;
    private MeshFilter      _meshFilter;
    private Texture2D       _texture;
    private Vector3[]       _minMax;
    private Vector3[]       _newVertsPos;
    private List<Vector3[]> _positionsLookup;

    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();

        _texture = bakedShapeData.texture;
        _minMax  = bakedShapeData.minMaxPos;
        _frames  = bakedShapeData.framesCount;

        _positionsLookup = new List<Vector3[]>(_frames);
        for (int f = 0; f < _frames; f++)
        {
            _positionsLookup.Add(new Vector3[_meshFilter.mesh.vertexCount]);
            for (int v = 0; v < _meshFilter.mesh.vertexCount; v++)
            {
                _positionsLookup[f][v] = GetPositionFromTexture(f * _meshFilter.mesh.vertexCount + v);
            }
        }

        _newVertsPos = new Vector3[_meshFilter.mesh.vertexCount];
    }

    private void Update()
    {
        _currentFrame = Mathf.FloorToInt(animationTime * (_frames - 1));

        int blendFrame = Mathf.CeilToInt(animationTime * (_frames - 1));

        _lerp = (Mathf.FloorToInt(animationTime * (_frames - 1)) - (animationTime * (_frames - 1))) * -1f;

        if (blendFrame == _frames - 1)
            blendFrame--;

        for (int v = 0; v < _meshFilter.mesh.vertexCount; v++)
        {
            _newVertsPos[v] = Vector3.Lerp(_positionsLookup[_currentFrame][v], _positionsLookup[blendFrame][v], _lerp);
        }

        _meshFilter.mesh.vertices = _newVertsPos;
        _meshFilter.mesh.RecalculateNormals();
    }

    private Vector3 GetPositionFromTexture(int index)
    {
        Vector2Int pixelCoord = BakeBlendShapeAnimation.GetPixelCoordByIndex(index, _texture.width);
        Color color           = _texture.GetPixel(pixelCoord.x, pixelCoord.y);

        float x = BakeBlendShapeAnimation.Remap(color.r, 0f, 1f, _minMax[0].x, _minMax[1].x);
        float y = BakeBlendShapeAnimation.Remap(color.g, 0f, 1f, _minMax[0].y, _minMax[1].y);
        float z = BakeBlendShapeAnimation.Remap(color.b, 0f, 1f, _minMax[0].z, _minMax[1].z);

        Vector3 pos = new Vector3(x, y, z);

        return pos;
    }
}
