using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class AI_Fov : MonoBehaviour
{
    [Range(0, 360)]
    public float _fov = 180f;
    public int _rayCount = 90;
    public float _viewDistance = 7.5f;
    public LayerMask _layerMask;
    private Mesh _mesh;
    private List<GameObject> _detectedObjects = new List<GameObject>();
    private void Awake()
    {
        _layerMask = LayerMask.GetMask("Obstacle", "Player");
    }

    private void Start()
    {
        _mesh = new Mesh { name = "FOV" };
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    private void LateUpdate()
    {
        DrawFov();
    }

    private void DrawFov()
    {
        _detectedObjects.Clear();

        float baseAngle = transform.localEulerAngles.z;
        float angle = baseAngle + _fov * 0.5f;
        float angleIncrease = _fov / _rayCount;

        Vector3[] vertices = new Vector3[_rayCount + 2];
        int[] triangles = new int[_rayCount * 3];

        vertices[0] = Vector3.zero;
        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= _rayCount; i++)
        {
            Vector3 vertex;
            Vector3 direction = GetVectorFromAngle(angle);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _viewDistance, _layerMask);

            if (hit.collider == null)
            {
                vertex = direction * _viewDistance;
            }
            else
            {
                vertex = direction * hit.distance;
                if (!_detectedObjects.Contains(hit.collider.gameObject))
                {
                    _detectedObjects.Add(hit.collider.gameObject);
                }
            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f);
    }

    public List<GameObject> GetDetectedObjects()
    {
        return _detectedObjects;
    }
}
