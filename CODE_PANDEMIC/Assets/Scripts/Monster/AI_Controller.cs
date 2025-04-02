using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AI_State2
{
    Idle,
    Walk,
    Attack,
    Dead
}

public class AI_Controller : MonoBehaviour
{
    private Transform _player;
    private Rigidbody2D _rb;
    private Coroutine _aiDamageCoroutine;
    private SpriteRenderer _renderer;
    
    private float _aiDamage = 10f;
    private float _aiDetectionRange = 7.5f;
    private float _aiDetectionAngle = 120f;
    private float _aiDamageDelay = 0.5f;
    
    private float _currentAngle;
    private float _currentDistance;
    
    public virtual bool Init()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("Player 없음");
            return false;
        }
        _player = playerObj.transform;
        
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody 없음");
            return false;
        }
        _rb.freezeRotation = true;
        
        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null)
        {
            Debug.LogError("Sprite 없음");
            return false;
        }
        
        return true;
    }
    
    void Start()
    {
        if (!Init())
        {
            enabled = false;
            return;
        }
    }
    
    private void Update()
    {
        if (_player == null)
            return;
        
        _currentAngle = CalculateAngle();
        _currentDistance = CalculateDistance();
    }
    
    private void FixedUpdate()
    {
        if (_player == null)
            return;
        
        if (_currentDistance <= _aiDetectionRange)
        {
            if (_currentAngle <= _aiDetectionAngle / 2)
            {
                ChasePlayer();
            }
            else
            {
                StopMoving();
            }
        }
        else
        {
            StopMoving();
        }
    }
    
    private float CalculateAngle()
    {
        Vector2 aiDirection = (_player.position - transform.position).normalized;
        Vector2 aiForward = (_player.position.x < transform.position.x) ? Vector2.left : Vector2.right;
        return Vector2.Angle(aiForward, aiDirection);
    }
    
    private float CalculateDistance()
    {
        return Vector2.Distance(_player.position, transform.position);
    }
    
    private void ChasePlayer()
    {
        // 좀비가 플레이어를 향하도록 스프라이트 flip 처리
        _renderer.flipX = _player.position.x < transform.position.x;
    }
    
    private void StopMoving()
    {
        // 추후 Idle 애니메이션 및 정지 로직 추가 예정
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _aiDamageCoroutine ??= StartCoroutine(AttackPlayer());
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && _aiDamageCoroutine != null)
        {
            StopCoroutine(_aiDamageCoroutine);
            _aiDamageCoroutine = null;
        }
    }
    
    private IEnumerator AttackPlayer()
    {
        Debug.Log(_aiDamage + " 데미지");
        // TODO: 플레이어 체력 감소 로직 추가
        yield return new WaitForSecondsRealtime(_aiDamageDelay);
    }
}
