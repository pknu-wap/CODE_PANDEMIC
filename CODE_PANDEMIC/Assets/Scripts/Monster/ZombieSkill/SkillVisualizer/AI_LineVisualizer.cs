using System.Collections.Generic;
using UnityEngine;

public class AI_LineVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    private float tileLength = 1f;

    private List<GameObject> _tiles = new List<GameObject>();

    public void Show(Vector2 origin, Vector2 target, float totalLength, float height = 1f)
    {
        Vector2 direction = (target - origin).normalized;
        int tileCount = Mathf.CeilToInt(totalLength / tileLength);

        while (_tiles.Count < tileCount)
        {
            var tile = Instantiate(tilePrefab, transform);
            _tiles.Add(tile);
        }

        for (int i = 0; i < _tiles.Count; i++)
        {
            bool active = i < tileCount;
            _tiles[i].SetActive(active);
            if (!active) continue;

            Vector2 pos = origin + direction * (tileLength * i + tileLength / 2f);
            _tiles[i].transform.position = pos;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _tiles[i].transform.rotation = Quaternion.Euler(0f, 0f, angle);

            Vector3 parentScale = transform.lossyScale;
            _tiles[i].transform.localScale = new Vector2(
                height / parentScale.x,
                height / parentScale.y
         );
        }

    }

    public void Hide()
    {
        foreach (var tile in _tiles)
            tile.SetActive(false);
    }
}
