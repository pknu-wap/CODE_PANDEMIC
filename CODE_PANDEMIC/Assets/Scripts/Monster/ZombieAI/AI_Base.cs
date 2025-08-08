using System;
using UnityEngine;

public abstract class AI_Base : MonoBehaviour
{
    protected MonsterData _monsterData;
    protected AI_State _state = AI_State.Idle;
    protected AI_Health _health;

    public event Action OnDie;

    protected virtual void Awake()
    {
        _health = GetComponent<AI_Health>();
    }

    protected virtual void OnEnable()
    {
        _health.OnDied += Die;
    }

    protected virtual void OnDisable()
    {
        _health.OnDied -= Die;
    }

    public virtual void SetInfo(MonsterData monsterData)
    {
        _monsterData = monsterData;
        _health.SetInfo(monsterData);
    }

    public virtual bool Init()
    {
        if (_monsterData == null)
        {
            _monsterData = new MonsterData
            {
                NameID = "TestZombie",
                Hp = 100,
                AttackDelay = 5.0f,
                DetectionRange = 7.5f,
                DetectionAngle = 120,
                MoveSpeed = 1.0f,
                AttackRange = 2f,
                AttackDamage = 10
            };
            _health.SetInfo(_monsterData);
        }
        return true;
    }

    public virtual void SetState(AI_State state)
    {
        _state = state;
    }

    public AI_State GetState()
    {
        return _state;
    }

    public virtual void TakeDamage(int amount)
    {
        _health.TakeDamage(amount);
    }

    public virtual void Die()
    {
        if (_state == AI_State.Dead)
            return;

        _state = AI_State.Dead;
        OnDie?.Invoke();
    }

    public virtual void DieAnimationEnd()
    {
        gameObject.SetActive(false);
    }

    public float MoveSpeed => _monsterData.MoveSpeed;
    public float DetectionRange => _monsterData.DetectionRange;
    public float DetectionAngle => _monsterData.DetectionAngle;
    public float Damage => _monsterData.AttackDamage;
    public int Health => _health.CurrentHp;
    public float AttackRange => _monsterData.AttackRange;
    public float AttackDelay => _monsterData.AttackDelay;
}