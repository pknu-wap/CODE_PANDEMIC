using System.Collections;
using UnityEngine;

public class AI_BossController : AI_Base
{
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _chaseRange = 10f;
    [SerializeField] private float _stateCheckInterval = 0.2f;

    private Transform _target;

    private void Start()
    {
        PlayerController playerComponent = FindObjectOfType<PlayerController>();
        _target = playerComponent.transform;
        if (_target == null)
        {
            enabled = false;
            return;
        }

        SetState(AI_State.Idle);
        StartCoroutine(CheckStateRoutine());
    }

    private void Update()
    {
        switch (_state)
        {
            case AI_State.Idle:
                break;

            case AI_State.Walk:
                ChaseTarget();
                break;

            case AI_State.Attack:
                break;

            case AI_State.Dead:
                break;
        }
    }

    private IEnumerator CheckStateRoutine()
    {
        while (_state != AI_State.Dead)
        {
            float distance = Vector2.Distance(transform.position, _target.position);

            if (distance <= _attackRange)
                SetState(AI_State.Attack);
            else if (distance <= _chaseRange)
                SetState(AI_State.Walk);
            else
                SetState(AI_State.Idle);

            yield return new WaitForSeconds(_stateCheckInterval);
        }
    }

    private void ChaseTarget()
    {
        if (_target == null) return;

        Vector2 direction = (_target.position - transform.position).normalized;
        transform.position += (Vector3)(_monsterData.MoveSpeed * Time.deltaTime * direction);
    }

    public override void Die()
    {
        base.Die();
        StopAllCoroutines();
        Debug.Log("보스 사망");
    }
}
