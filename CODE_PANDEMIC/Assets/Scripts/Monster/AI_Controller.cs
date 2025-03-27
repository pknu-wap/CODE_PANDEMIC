using System.Collections;
using System.Collections.Generic;
using UnityEngine;

private enum AI_State
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

    private float _aiSpeed = 2f;
    private float _aiDamage = 10f;
    private float _aiDetectionRange = 7.5f; // 시야 거리
    private float _aiDetectionAngle = 120f; // 시야각
    private float _aiDamageDelay = 0.5f; // 데미지 딜레이(닿였을때 대미지 들어오는 간격)
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
        enabled = false; //MonoBehaviour 내장 함수 , 오류 있을시 비활성화
        return;
        }
    }

    private void Update(){
        float currentAngle = AI_CalculateAngle();
        float currentDistance = AI_CalculateDistance();
    }
    private void FixedUpdate()
    {
        if (_player == null)
            return;

        float currentAngle = AI_CalculateAngle();
        float currentDistance = AI_CalculateDistance();

        if (currentAngle <= _aiDetectionAngle / 2) // 시야각 계산 
        {
            if (currentDistance <= _aiDetectionRange)
            {
                ChasePlayer(_player.position);
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
        // 플레이어가 왼쪽에 있으면 왼쪽, 오른쪽이면 오른쪽을 전방으로 설정
        return Vector2.Angle(aiForward, aiDirection);
    }

    private float AI_CalculateDistance()
    {
        return Vector2.Distance(_player.position, transform.position);
    }

    private void ChasePlayer(Vector3 target)
    {
        Vector2 newPosition = Vector2.MoveTowards(_Rb.position, target, _aiSpeed * Time.fixedDeltaTime);
        _Rb.MovePosition(newPosition);
        _Renderer.flipX = _player.position.x < transform.position.x; // 플레이어가 왼쪽에 있을 때 왼쪽 보게함 
    }

    private void AI_StopMoving()
    {
        // 추후 Idle 애니메이션 추가 예정
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            _aiDamageCoroutine ??= StartCoroutine(AI_Damage());
        }   
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_aiDamageCoroutine != null)
            {
                StopCoroutine(_aiDamageCoroutine);
                _aiDamageCoroutine = null;
                
            }

        }
    }

    private IEnumerator AI_Damage()
    {
            Debug.Log(_aiDamage + " 데미지");
            // 플레이어 체력 감소 로직 추가 예정 
            yield return new WaitForSecondsRealtime(_aiDamageDelay);
    }

}
