using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI_Base : MonoBehaviour
{
    // 기본 체력, 데미지, 이동 속도, 감지 범위, 시야각 등 공통 속성
    [SerializeField] protected float _aiHealth = 100f;
    [SerializeField] protected float _aiDamage = 10f;
    [SerializeField] protected float _aiMoveSpeed = 200f;
    [SerializeField] protected float _aiDetectionRange = 15f;
    [SerializeField] protected float _aiDetectionAngle = 120f;
    [SerializeField] protected float _aiDamageDelay = 0.5f; 

    protected AI_State _state = AI_State.Idle;

    /// <summary>
    /// AI 초기화를 위한 공통 메서드. 하위 클래스에서 추가로 초기화할 수 있습니다.
    /// </summary>
    public virtual bool Init()
    {
        // 기본 초기화 작업 (예: 컴포넌트 캐싱 등)
        _state = AI_State.Idle;
        return true;
    }

    /// <summary>
    /// AI 상태를 설정합니다.
    /// </summary>
    public virtual void SetState(AI_State state)
    {
        _state = state;
    }

    /// <summary>
    /// 현재 AI 상태를 반환합니다.
    /// </summary>
    public AI_State GetState()
    {
        return _state;
    }

    /// <summary>
    /// AI가 데미지를 받을 때 호출합니다.
    /// </summary>
    public virtual void TakeDamage(float amount)
    {
        _aiHealth -= amount;
        Debug.Log("AI 체력: " + _aiHealth);
        if (_aiHealth <= 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// AI가 사망할 때 호출됩니다.
    /// </summary>
    protected virtual void Die()
    {
        _state = AI_State.Dead;
        Debug.Log("AI 사망");
        // 추가 처리: 애니메이션 재생, 콜라이더 비활성화, 게임 오브젝트 제거 등
        // 예: Destroy(gameObject);
    }

    /// <summary>
    /// AI의 이동 속도, 감지 범위 등 공통 프로퍼티에 접근하는 프로퍼티들
    /// </summary>
    public float MoveSpeed { get { return _aiMoveSpeed; } }
    public float DetectionRange { get { return _aiDetectionRange; } }
    public float DetectionAngle { get { return _aiDetectionAngle; } }
    public float Damage { get { return _aiDamage; } }
    public float Health { get { return _aiHealth; } }
}