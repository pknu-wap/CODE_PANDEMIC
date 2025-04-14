using Pathfinding;
using UnityEngine;

public class AI_AstarControl : MonoBehaviour
{
    private float _radius = 1.75f;
    private float _height = 0.01f;
    private float _speed = 3.0f;
    private float _pickNextWaypointDist = 1.2f;
    private Vector3 _gravity = new(0, 0, 0);

    
    private int _gridWidth = 50;
    private int _gridDepth = 30;
    private float _nodeSize = 0.5f;
    private float _collisionDiameter = 1.3f;
    private string _obstacleLayerName = "Obstacle";

    private void Start()
    {
        ConfigureAllAIPaths();
        AssignDestinations();
        ConfigureGridGraph();
    }

    private void ConfigureAllAIPaths()
    {
        AIPath[] allAiPaths = FindObjectsOfType<AIPath>();
        foreach (AIPath aiPath in allAiPaths)
        {
            aiPath.radius = _radius;
            aiPath.height = _height;
            aiPath.maxSpeed = _speed;
            aiPath.pickNextWaypointDist = _pickNextWaypointDist;
            aiPath.orientation = OrientationMode.YAxisForward; // 2D 모드
            aiPath.enableRotation = false;
            aiPath.gravity = _gravity;
        }
    }

    private void AssignDestinations()
    {
        AIDestinationSetter[] allDestinations = FindObjectsOfType<AIDestinationSetter>();
        PlayerController playerComponent = FindObjectOfType<PlayerController>();

        foreach (AIDestinationSetter destinationSetter in allDestinations)
        {
            destinationSetter.target = playerComponent.transform;
        }
    }

    private void ConfigureGridGraph()
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        gridGraph.SetDimensions(_gridWidth, _gridDepth, _nodeSize);
        gridGraph.is2D = true;
        gridGraph.center = Vector3.zero;
        gridGraph.collision.use2D = true;
        gridGraph.collision.diameter = _collisionDiameter;
        gridGraph.collision.mask = LayerMask.GetMask(_obstacleLayerName);

        // 변경된 설정을 반영
        AstarPath.active.Scan();
    }
}
