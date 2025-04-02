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
    private Rigidbody2D _Rb;
    private Coroutine _aiDamageCoroutine;
    private SpriteRenderer _Renderer;

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
        
        _Rb = GetComponent<Rigidbody2D>();
        if (_Rb == null)
        {
            Debug.LogError("Rigidbody 없음");
            return false;
        }
        _Rb.freezeRotation = true;

        _Renderer = GetComponent<SpriteRenderer>();
        if (_Renderer == null)
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
        
        _currentAngle = AI_CalculateAngle();
        _currentDistance = AI_CalculateDistance();
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
                AI_StopMoving();
            }
        }
        else
        {
            AI_StopMoving();
        }
    }

    private float AI_CalculateAngle()
    {
        Vector2 aiDirection = (_player.position - transform.position).normalized;
        Vector2 aiForward = (_player.position.x < transform.position.x) ? Vector2.left : Vector2.right;
        return Vector2.Angle(aiForward, aiDirection);
    }

    private float AI_CalculateDistance()
    {
        return Vector2.Distance(_player.position, transform.position);
    }

    private void ChasePlayer()
    {
        _Renderer.flipX = _player.position.x < transform.position.x;
    }



    private void AI_StopMoving()
    {
        // 추후 Idle 애니메이션 및 정지 로직 추가 예정
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _aiDamageCoroutine ??= StartCoroutine(ZombieAttackPlayer());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _aiDamageCoroutine != null)
        {
            StopCoroutine(_aiDamageCoroutine);
            _aiDamageCoroutine = null;
        }
    }

    private IEnumerator ZombieAttackPlayer()
    {
        Debug.Log(_aiDamage + " 데미지");
        // TODO: 플레이어 체력 감소 로직 추가
        yield return new WaitForSecondsRealtime(_aiDamageDelay);
    }

}
