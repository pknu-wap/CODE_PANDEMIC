using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI_Base : MonoBehaviour
{
    // 기본 체력, 데미지, 이동 속도, 감지 범위, 시야각 등 공통 속성
    [SerializeField] protected float _aiHealth;
    [SerializeField] protected float _aiDamage;
    [SerializeField] protected float _aiMoveSpeed;
    [SerializeField] protected float _aiDetectionRange;
    [SerializeField] protected float _aiDetectionAngle;
    [SerializeField] protected float _aiDamageDelay; 
    [SerializeField] protected string _aiName;

    protected AI_State _state = AI_State.Idle;


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

    protected virtual void Die()
    {
        _state = AI_State.Dead;
        Debug.Log("AI 사망");
            }
    public float MoveSpeed { get { return _aiMoveSpeed; } }
    public float DetectionRange { get { return _aiDetectionRange; } }
    public float DetectionAngle { get { return _aiDetectionAngle; } }
    public float Damage { get { return _aiDamage; } }
    public float Health { get { return _aiHealth; } }
}