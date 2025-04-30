using Pathfinding;
using UnityEngine;

public class AI_AstarControl : MonoBehaviour
{
    private float _radius = 0.41f;
    private float _height = 0.01f;
    private float _speed = 1.0f;
    private float _pickNextWaypointDist = 1.2f;
    private Vector3 _gravity = new(0, 0, 0);
    
    private int _gridWidth = 72;
    private int _gridDepth = 46;
    private float _nodeSize = 1.0f;
    private float _collisionDiameter = 0.85f;
    private string _obstacleLayerName = "Obstacle";

    private void Awake()
    {
        ConfigureGridGraph();
    }


    private void ConfigureGridGraph()
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        gridGraph.SetDimensions(_gridWidth, _gridDepth, _nodeSize);
        gridGraph.is2D = true;
        gridGraph.collision.use2D = true;
        gridGraph.collision.diameter = _collisionDiameter;
        gridGraph.collision.mask = LayerMask.GetMask(_obstacleLayerName);

        // 변경된 설정을 반영
        AstarPath.active.Scan();
    }
}
