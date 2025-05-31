using UnityEngine;

public class AI_BossThrowVisualizer : AI_LineVisualizer
{
    private AI_HospitalBoss _bossController;

    public void SetController(AI_HospitalBoss bossController)
    {
        _bossController = bossController;
    }

    public override void Show(Vector2 origin, Vector2 target, float totalLength, float height = 1f)
    {

        float[] angles = _bossController.IsBerserk
            ? new float[] { +45f, +15f, -15f, -45f }
            : new float[] { +45f, 0f, -45f };

        Vector2 baseDirection = (target - origin).normalized;
        int neededTileCount = Mathf.CeilToInt(totalLength / tileLength);

        while (_tiles.Count < neededTileCount * angles.Length)
        {
            var tile = Instantiate(tilePrefab, transform);
            _tiles.Add(tile);
        }

        for (int i = 0; i < _tiles.Count; i++)
        {
            _tiles[i].SetActive(false);
        }

        for (int angleIndex = 0; angleIndex < angles.Length; angleIndex++)
        {
            float angleOffset = angles[angleIndex];
            float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
            float arrowAngle = baseAngle + angleOffset;
            Vector2 direction = new Vector2(Mathf.Cos(arrowAngle * Mathf.Deg2Rad), Mathf.Sin(arrowAngle * Mathf.Deg2Rad));

            for (int i = 0; i < neededTileCount; i++)
            {
                int tileIndex = angleIndex * neededTileCount + i;
                _tiles[tileIndex].SetActive(true);

                Vector2 pos = origin + direction * (tileLength * i + tileLength / 2f);
                _tiles[tileIndex].transform.position = pos;

                _tiles[tileIndex].transform.rotation = Quaternion.Euler(0f, 0f, arrowAngle);

                Vector3 parentScale = transform.lossyScale;
                _tiles[tileIndex].transform.localScale = new Vector2(
                    height / parentScale.x,
                    height / parentScale.y
                );
            }
        }
    }

    public override void Hide()
    {
        base.Hide();
    }
}
