using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AIPath), typeof(AIDestinationSetter))]
public class AI_Movement : MonoBehaviour
{
    private AIPath _aiPath;
    private AIDestinationSetter _destinationSetter;
    private AI_Fov _aiFov;

    public bool _isUsingSkill = false;

    private void Awake()
    {
        _aiPath = GetComponent<AIPath>();
        _destinationSetter = GetComponent<AIDestinationSetter>();
        _aiFov = GetComponentInChildren<AI_Fov>();
    }

    public void Configure(float moveSpeed)
    {
        _aiPath.maxSpeed = moveSpeed;
        _aiPath.height = 0.01f;
        _aiPath.pickNextWaypointDist = 1.2f;
        _aiPath.orientation = OrientationMode.YAxisForward;
        _aiPath.enableRotation = false;
        _aiPath.gravity = Vector3.zero;
        _destinationSetter.target = null;
    }

    public void SetTarget(Transform target)
    {
        _destinationSetter.target = target;
    }

    public void ChasePlayer()
    {
        _aiPath.canMove = true;
    }

    public void StopMoving()
    {
        _aiPath.canMove = false;
    }

    public void UpdateDirection(Transform target)
    {
        if (target == null || _isUsingSkill) return;

        float direction = target.position.x - transform.position.x;
        Vector3 scale = transform.localScale;
        scale.x = direction > 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void UpdateFovDirection()
    {
        if (_aiFov == null) return;
        float angle = transform.localScale.x < 0 ? 0f : 180f;
        _aiFov.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void SetSkillState(bool isUsingSkill)
    {
        _isUsingSkill = isUsingSkill;
    }
}