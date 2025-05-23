using Pathfinding;
using UnityEngine;

public class AI_AstarControl : MonoBehaviour
{
    private float _collisionDiameter = 0.85f;
    private string _obstacleLayerName = "Wall";

    private void Awake()
    {
        ConfigureGridGraph();
    }


    private void ConfigureGridGraph()
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        gridGraph.is2D = true;
        gridGraph.collision.use2D = true;
        gridGraph.collision.diameter = _collisionDiameter;
        gridGraph.collision.mask = LayerMask.GetMask(_obstacleLayerName);
        gridGraph.drawGizmos = false;

        // 변경된 설정을 반영
        AstarPath.active.Scan();
    }
}
