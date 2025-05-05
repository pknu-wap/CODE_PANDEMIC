using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class AI_ThrowVisualizer : MonoBehaviour
{
    public Material fillMaterial;
    public float fadeDuration = 0.5f;

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Mesh _mesh;
    private Coroutine _fillCoroutine;

    private GameObject _outlineObject;
    private MeshRenderer _outlineRenderer;
    private MeshFilter _outlineFilter;

    [SerializeField] private AI_NurseZombie _nurseZombie;

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

        _outlineObject = new GameObject("RectOutline");
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

    public void Show(Vector2 targetPosition, float duration)
{
    Vector2 origin = _nurseZombie.transform.position;
    Vector2 direction = (targetPosition - origin).normalized;
    float maxRange = _nurseZombie.SyringeRange;

    transform.position = origin;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0f, 0f, angle);
    transform.localScale = new Vector3(Mathf.Sign(_nurseZombie.transform.localScale.x), 1f, 1f);

    float height = 0.5f;
    float width = maxRange;

    Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;

    float directionSign = Mathf.Sign(_nurseZombie.transform.localScale.x);
    float signedWidth = width * directionSign;

    _outlineFilter.mesh = GenerateRectMesh(height, Mathf.Abs(signedWidth));

    if (_fillCoroutine != null)
        StopCoroutine(_fillCoroutine);
    _fillCoroutine = StartCoroutine(AnimateFill(duration, height, signedWidth));

    transform.localScale = new Vector3(1f / parentScale.x, 1f / parentScale.y, 1f);

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

    private IEnumerator AnimateFill(float duration, float height, float fullWidth)
    {
        float elapsed = 0f;
        float absWidth = Mathf.Abs(fullWidth);

        while (elapsed < duration)
        {
            float factor = elapsed / duration;
            float width = absWidth * factor;
            _meshFilter.mesh = GenerateRectMesh(height, width);
            _meshRenderer.material.color = new Color(1f, 0f, 0f, factor);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _meshFilter.mesh = GenerateRectMesh(height, absWidth);
        _meshRenderer.material.color = new Color(1f, 0f, 0f, 0.5f);
    }

    private Mesh GenerateRectMesh(float height, float width)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0f, -height * 0.5f, 0f),        // bottom-left
            new Vector3(width, -height * 0.5f, 0f),     // bottom-right
            new Vector3(width, height * 0.5f, 0f),      // top-right
            new Vector3(0f, height * 0.5f, 0f)          // top-left
        };

        int[] triangles = new int[] { 0, 1, 2, 2, 3, 0 };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}
