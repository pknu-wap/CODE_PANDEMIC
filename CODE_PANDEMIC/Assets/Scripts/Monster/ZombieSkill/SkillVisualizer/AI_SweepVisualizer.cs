using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class AI_SweepVisualizer : MonoBehaviour
{
    public AI_DoctorZombie _doctor;
    public Transform playerTransform;

    public float fadeDuration = 0.5f;

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Mesh _mesh;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;

        Material material = new Material(Shader.Find("Sprites/Default"));
        material.color = new Color(1f, 0f, 0f, 0f);
        _meshRenderer.material = material;
    }
    public void Show(float chargeTime)
    {
        Vector2 diff = playerTransform.position - transform.position;
        Vector2 cardinalDir;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            cardinalDir = (diff.x > 0) ? Vector2.right : Vector2.left;
        }
        else
        {
            cardinalDir = (diff.y > 0) ? Vector2.up : Vector2.down;
        }

        transform.rotation = Quaternion.FromToRotation(Vector2.up, cardinalDir);

        float angle = 90f;  
        float radius = _doctor.SweepRange * 3f; // 시각적 효과 고려
        GenerateMesh(angle, radius);

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeIn(chargeTime));
    }

    public void Hide()
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeOut());
    }

   
    private void GenerateMesh(float angle, float radius)
    {

        int sweepCount = _doctor.SweepCount;
        float deltaAngle = angle / sweepCount;
        float startAngle = -angle * 0.5f;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();


        vertices.Add(Vector3.zero);

        for (int i = 0; i <= sweepCount; i++)
        {
            float currentAngle = startAngle + deltaAngle * i;
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 point = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f) * radius;
            vertices.Add(point);
        }

        for (int i = 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        _mesh.Clear();
        _mesh.vertices = vertices.ToArray();
        _mesh.triangles = triangles.ToArray();

        Vector2[] uvs = new Vector2[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 v = vertices[i];
            uvs[i] = new Vector2(v.x / radius * 0.5f + 0.5f, v.y / radius * 0.5f + 0.5f);
        }
        _mesh.uv = uvs;
        _mesh.RecalculateNormals();
    }

    private IEnumerator FadeIn(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Color color = new Color(1f, 0f, 0f, t);
            _meshRenderer.material.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }
        _meshRenderer.material.color = new Color(1f, 0f, 0f, 1f);
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color startColor = _meshRenderer.material.color;

        while (elapsed < fadeDuration)
        {
            float t = 1f - (elapsed / fadeDuration);
            Color color = new Color(startColor.r, startColor.g, startColor.b, t);
            _meshRenderer.material.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }
        _meshRenderer.material.color = new Color(1f, 0f, 0f, 0f);
    }
}
