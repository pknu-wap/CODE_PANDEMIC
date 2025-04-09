using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI_Base : MonoBehaviour
{
    // 기본 체력, 데미지, 이동 속도, 감지 범위, 시야각 등 공통 속성
    protected float _aiHealth;
    protected float _aiDamage;
    protected float _aiMoveSpeed;
    protected float _aiDetectionRange;
    protected float _aiDetectionAngle;
    protected float _aiDamageDelay; 
    protected float _aiAttackRange;
    protected string _aiName;

    

    protected AI_State _state = AI_State.Idle;
    public event System.Action OnDie;

    public virtual bool Init()
    {
        _state = AI_State.Idle;
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

    public virtual void TakeDamage(float amount)
    {
        _aiHealth -= amount;
        Debug.Log("AI 체력: " + _aiHealth);
        if (_aiHealth <= 0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        _state = AI_State.Dead;
        OnDie?.Invoke(); 
        gameObject.SetActive(false);
    }
    public float MoveSpeed { get { return _aiMoveSpeed; } }
    public float DetectionRange { get { return _aiDetectionRange; } }
    public float DetectionAngle { get { return _aiDetectionAngle; } }
    public float Damage { get { return _aiDamage; } }
    public float Health { get { return _aiHealth; } }
}