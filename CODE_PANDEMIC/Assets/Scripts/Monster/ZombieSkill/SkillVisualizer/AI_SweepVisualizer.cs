using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class AI_SweepVisualizer : MonoBehaviour
{
    public int segmentCount = 30;
    public Material material;
    public float fadeDuration = 0.5f;

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Mesh _mesh;
    private Coroutine _fillCoroutine;
    private GameObject _outlineObject;
    private MeshRenderer _outlineRenderer;
    private MeshFilter _outlineFilter;

    private void Awake()
{
    _meshRenderer = GetComponent<MeshRenderer>();
    _meshFilter = GetComponent<MeshFilter>();
    _mesh = new Mesh();
    _meshFilter.mesh = _mesh;

    Material mat = new Material(Shader.Find("Sprites/Default"));
    mat.color = new Color(1f, 0f, 0f, 0f);
    _meshRenderer.material = mat;
    _meshRenderer.enabled = false;
    mat.renderQueue = 4000;

    _outlineObject = new GameObject("SweepOutline");
    _outlineObject.transform.SetParent(transform);
    _outlineObject.transform.localPosition = Vector3.zero;
    _outlineObject.transform.localRotation = Quaternion.identity;
    _outlineObject.transform.localScale = Vector3.one;

    _outlineRenderer = _outlineObject.AddComponent<MeshRenderer>();
    _outlineFilter = _outlineObject.AddComponent<MeshFilter>();

    Material outlineMat = new Material(Shader.Find("Sprites/Default"));
    outlineMat.color = new Color(1f, 0f, 0f, 0.2f);
    _outlineRenderer.material = outlineMat;
    outlineMat.renderQueue = 4000;
}

    public void Show(Vector2 forward, float fullAngle, float fullRadius, float chargeTime)
    {
        Vector2 snapped = SnapDirection(forward);
        transform.rotation = Quaternion.FromToRotation(Vector2.up, snapped);

        Mesh outlineMesh = GenerateMesh(fullAngle, fullRadius);
        _outlineFilter.mesh = outlineMesh;

        if (_fillCoroutine != null)
            StopCoroutine(_fillCoroutine);
        _fillCoroutine = StartCoroutine(AnimateFill(chargeTime, fullAngle, fullRadius));
        _meshRenderer.enabled = true;
        _outlineRenderer.enabled = true;
        
    }

    public void Hide()
    {
        if (_fillCoroutine != null)
        {
            StopCoroutine(_fillCoroutine);
            _fillCoroutine = null;
        }
        _meshRenderer.enabled = false;
        _meshRenderer.material.color = new Color(1f, 0f, 0f, 0f);
        _outlineRenderer.enabled = false;
    }

    private IEnumerator AnimateFill(float duration, float fullAngle, float fullRadius)
{
    float elapsed = 0f;
    while (elapsed < duration)
    {
        float fillFactor = elapsed / duration;
        Mesh mesh = GenerateMesh(fullAngle, fullRadius * fillFactor);
        _meshFilter.mesh = mesh;
        _meshRenderer.material.color = new Color(1f, 0f, 0f, fillFactor);
        elapsed += Time.deltaTime;
        yield return null;
    }
    Mesh finalMesh = GenerateMesh(fullAngle, fullRadius);
    _meshFilter.mesh = finalMesh;
    _meshRenderer.material.color = new Color(1f, 0f, 0f, 0.5f);
    
}

    public Mesh GenerateMesh(float angle, float radius)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3> { Vector3.zero };
        List<int> triangles = new List<int>();

        if (segmentCount <= 0)
            segmentCount = 30;
        float deltaAngle = angle / segmentCount;
        float startAngle = -angle * 0.5f;

        for (int i = 0; i <= segmentCount; i++)
        {
            float currentAngle = startAngle + deltaAngle * i;
            float rad = currentAngle * Mathf.Deg2Rad;
            vertices.Add(new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0));
        }

        for (int i = 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        return mesh;
    }

    private Vector2 SnapDirection(Vector2 dir)
    {
        Vector2[] candidates = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        float maxDot = -Mathf.Infinity;
        Vector2 best = Vector2.up;
        foreach (var candidate in candidates)
        {
            float dot = Vector2.Dot(dir.normalized, candidate);
            if (dot > maxDot)
            {
                maxDot = dot;
                best = candidate;
            }
        }
        return best;
    }
}