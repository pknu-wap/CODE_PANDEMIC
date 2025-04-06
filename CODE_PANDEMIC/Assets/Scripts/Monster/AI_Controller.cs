using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public enum AI_State
{
    Idle,
    Walk,
    Attack,
    Dead
}

public class AI_Controller : AI_Base
{
    private Transform _player;
    private Rigidbody2D _rb;
    private Coroutine _aiDamageCoroutine;
    private SpriteRenderer _renderer;
    private AI_Fov _aiFov;
    private AIPath _aiPath;


    public override bool Init()
    {

        PlayerMovement playerComponent = FindObjectOfType<PlayerMovement>();
        if (playerComponent == null)
        {
            Debug.LogError("Player 없음");
            return false;
        }
        _player = playerComponent.transform;

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

        _aiFov = GetComponentInChildren<AI_Fov>();
        if (_aiFov == null)
        {
            Debug.LogError("AI_Fov 컴포넌트를 찾을 수 없습니다.");
            return false;
        }

        _aiPath = GetComponent<AIPath>();
        if (_aiPath == null)
        {
            Debug.LogError("AIPath 없음");
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
        UpdateFovDirection();
    }

    private void FixedUpdate()
    {
        if (_player == null)
            return;

        _renderer.flipX = _player.position.x < transform.position.x;
        if (_aiFov.GetDetectedObjects().Contains(_player.gameObject))
        {
            ChasePlayer();
        }
        else
        {
            StopMoving();
        }
    }

    private void UpdateFovDirection()
    {
        if (_aiFov != null)
        {
            // _renderer.flipX가 true이면 좀비가 왼쪽을 바라보므로 FOV 자식 오브젝트의 localRotation을 180°로 설정
            float angle = _renderer.flipX ? 180f : 0f;
            _aiFov.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void ChasePlayer()
    {
        _aiPath.canMove = true;
    }

    private void StopMoving()
    {
        _aiPath.canMove = false;
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
