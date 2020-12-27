using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeBlendShapeAnimation : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("the number of frames to sample from the animation clip")]
    public int                  bakeFrameCount;
    public int                  clipFrameCount;
    [Tooltip("The animation state in the animation controller to be captured")]
    public string               animationStateName;
    public BakedShapeData       bakedShapeData;
    public string               fileName;
    public string               path;


    private SkinnedMeshRenderer _skin;
    private Animator            _animator;

    private Coroutine           _baker;
    private List<Vector3[]>     _mapedFramesVertices;
    private Mesh[]               _bakedMeshes;

    private Vector3[]           _minMaxPos;
    private Texture2D           _texture;
    private Texture2D           _textureVertIndices;


    void Start()
    {
        _skin                 = GetComponent<SkinnedMeshRenderer>();
        _animator             = GetComponent<Animator>();

        _bakedMeshes          = new Mesh[bakeFrameCount];
        _mapedFramesVertices  = new List<Vector3[]>();

        if (bakedShapeData == null)
            Debug.LogWarning("No 'Baked Shape Data' scriptable object was assigned, please create one and assign it to this component");
    }

    public void BakeFrames()
    {
        if (path == "")
        {
            Debug.LogWarning("Please choose a save path");
            return;
        }

        Mesh mesh = new Mesh();
        _skin.BakeMesh(mesh);

        InitializeTexture(mesh);

        if (_baker == null)
        {
            _animator.enabled = true;
            _baker = StartCoroutine(Baker());
        }
    }

    IEnumerator Baker()
    {
        _animator.speed = 0f;

        yield return new WaitForEndOfFrame();
        Debug.Log("Starting baker coroutine");
        yield return new WaitForEndOfFrame();

        int frame = 0;
        while (frame < bakeFrameCount)
        {
            _animator.Play(animationStateName, 0, (1f / clipFrameCount) * frame);

            yield return new WaitForEndOfFrame();

            _bakedMeshes[frame] = new Mesh();

            _skin.BakeMesh (_bakedMeshes[frame]);

            frame++;
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Got " + _bakedMeshes.Length + " frame meshes");
        yield return new WaitForEndOfFrame();

        GetMinMaxPositions(_bakedMeshes);

        for (int m = 0; m < _bakedMeshes.Length; m++)
        {
            Debug.Log("Mapping positions for frame # " + m);
            yield return new WaitForEndOfFrame();

            MapVerticesPos(_bakedMeshes[m]);
        }

        Debug.Log("Finished mapping positions");

        CreateTextures(_bakedMeshes);

        bakedShapeData.texture     = _texture;
        bakedShapeData.minMaxPos   = _minMaxPos;
        bakedShapeData.framesCount = bakeFrameCount;

        yield return null;
    }

    private void CreateTextures(Mesh[] meshes)
    {
        for (int m = 0; m < meshes.Length; m++)
        {
            for (int v = 0; v < _mapedFramesVertices[m].Length; v++)
            {
                Color color = new Color(_mapedFramesVertices[m][v].x,
                                        _mapedFramesVertices[m][v].y,
                                        _mapedFramesVertices[m][v].z);

                Vector2Int pixelCoord = GetPixelCoordByIndex(m * meshes[0].vertexCount + v, _texture.width); // index = currentFrame * vertexCount + currentVertex

                _texture.SetPixel(pixelCoord.x, pixelCoord.y, color);
            }
            _texture.Apply(); 
        }

        Debug.Log("Saving textures");
        SaveTextureAsPNG(_texture, path, fileName);
    }

    private void InitializeTexture(Mesh mesh)
    {
        int size = Mathf.CeilToInt(Mathf.Sqrt(mesh.vertexCount * bakeFrameCount)); // animation
        _texture = new Texture2D(size, size, TextureFormat.RGB24, false);
        Debug.Log("Texture initialized, size: " + size + "x" + size);
    }

    private void GetMinMaxPositions(Mesh[] meshs)
    {
        Debug.Log("Getting min max positions");
        float minX = Mathf.Infinity;
        float minY = Mathf.Infinity;
        float minZ = Mathf.Infinity;

        float maxX = Mathf.NegativeInfinity;
        float maxY = Mathf.NegativeInfinity;
        float maxZ = Mathf.NegativeInfinity;

        for (int m = 0; m < meshs.Length; m++)
        {
            meshs[m].RecalculateBounds();

            if (meshs[m].bounds.min.x < minX) minX = meshs[m].bounds.min.x;
            if (meshs[m].bounds.min.y < minY) minY = meshs[m].bounds.min.y;
            if (meshs[m].bounds.min.z < minZ) minZ = meshs[m].bounds.min.z;

            if (meshs[m].bounds.max.x > maxX) maxX = meshs[m].bounds.max.x;
            if (meshs[m].bounds.max.y > maxY) maxY = meshs[m].bounds.max.y;
            if (meshs[m].bounds.max.z > maxZ) maxZ = meshs[m].bounds.max.z;
        }

        _minMaxPos = new Vector3[] { new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ) };
        Debug.Log("Min: " + _minMaxPos[0].x.ToString() + " , " + _minMaxPos[0].y.ToString() + " , " + _minMaxPos[0].z.ToString());
        Debug.Log("Max: " + _minMaxPos[1].x.ToString() + " , " + _minMaxPos[1].y.ToString() + " , " + _minMaxPos[1].z.ToString());
    }

    private void MapVerticesPos(Mesh mesh)
    {
        Vector3[] mapedMeshVertices = new Vector3[mesh.vertices.Length];
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            float x = Remap(mesh.vertices[i].x, _minMaxPos[0].x, _minMaxPos[1].x, 0f, 1f);
            float y = Remap(mesh.vertices[i].y, _minMaxPos[0].y, _minMaxPos[1].y, 0f, 1f);
            float z = Remap(mesh.vertices[i].z, _minMaxPos[0].z, _minMaxPos[1].z, 0f, 1f);

            mapedMeshVertices[i] = new Vector3(x, y, z);
        }
        
        _mapedFramesVertices.Add(mapedMeshVertices);
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static Vector2Int GetPixelCoordByIndex(int index, int width)
    {
        return new Vector2Int(index / width, index % width);
    }

    public void SaveTextureAsPNG(Texture2D texture, string path, string fileName)
    {
        byte[] _bytes = texture.EncodeToPNG();
        path     += "/" + fileName + ".png";

        System.IO.File.WriteAllBytes(path, _bytes);
        Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + path);
    }
}
