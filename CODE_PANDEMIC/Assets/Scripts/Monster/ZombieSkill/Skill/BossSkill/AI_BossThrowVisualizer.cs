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
        int totalTileCount = neededTileCount * angles.Length;

        while (_tiles.Count < totalTileCount)
            _tiles.Add(Instantiate(tilePrefab, transform));

        Hide();

        Vector3 parentScale = transform.lossyScale;

        for (int angleIndex = 0; angleIndex < angles.Length; angleIndex++)
        {
            float angleOffset = angles[angleIndex];
            Vector2 direction = Quaternion.Euler(0f, 0f, angleOffset) * baseDirection;
            float finalAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            for (int i = 0; i < neededTileCount; i++)
            {
                int tileIndex = angleIndex * neededTileCount + i;
                var tile = _tiles[tileIndex];
                tile.SetActive(true);

                Vector2 pos = origin + direction * (tileLength * i + tileLength / 2f);
                tile.transform.position = pos;
                tile.transform.rotation = Quaternion.Euler(0f, 0f, finalAngle);
                tile.transform.localScale = new Vector2(
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
