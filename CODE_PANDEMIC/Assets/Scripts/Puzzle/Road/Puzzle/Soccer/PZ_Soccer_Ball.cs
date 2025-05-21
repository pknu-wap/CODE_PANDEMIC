using UnityEngine;
using System;
using System.Collections;

public class PZ_Soccer_Ball : PZ_Interact_NonSpawn
{
    [SerializeField] private PZ_Soccer _Soccer;
    [SerializeField] private Rigidbody2D _rigidbody;

    [SerializeField] private GameObject _clearObject;
    [SerializeField] private GameObject _arrowUp;
    [SerializeField] private GameObject _arrowLeft;
    [SerializeField] private GameObject _arrowRight;

    public static event Action KickBallStart;
    public static event Action ResetBall;

    bool _isKicked = false;

    private Vector2 _direction;

    public override void Interact(GameObject player)
    {
        if (!_isKicked)
        {
            StartCoroutine(KickBall());
        }
        else
        {
            _rigidbody.position = new Vector2(69.41f, 97.66f);

            ResetBall?.Invoke();
        }

        _isKicked = !_isKicked;
    }

    private IEnumerator CheckDirection()
    {
        Vector2 spawnPos = _rigidbody.position;
        spawnPos.y += 0.7f;

        spawnPos.x -= 1;
        GameObject trash1 = Instantiate(_arrowLeft, spawnPos, Quaternion.identity);

        spawnPos.x += 1;
        GameObject trash2 = Instantiate(_arrowUp, spawnPos, Quaternion.identity);

        spawnPos.x += 1;
        GameObject trash3 = Instantiate(_arrowRight, spawnPos, Quaternion.identity);

        yield return new WaitUntil(() =>
        Input.GetKeyDown(KeyCode.W) ||
        Input.GetKeyDown(KeyCode.A) ||
        Input.GetKeyDown(KeyCode.D)
        );

        if (Input.GetKeyDown(KeyCode.W))
        {
            _direction = new Vector2(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _direction = new Vector2(-0.5f, 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _direction = new Vector2(0.5f, 1);
        }

        Destroy(trash1);
        Destroy(trash2);
        Destroy(trash3);
    }

    // 차 움직이기
    private IEnumerator KickBall()
    {
        yield return StartCoroutine(CheckDirection()); // 입력 대기

        KickBallStart?.Invoke();

        Vector2 startPos = _rigidbody.position; // 시작점
        Vector2 targetPos = _rigidbody.position + _direction * 6; // 도착점

        float currentTime = 0f;
        float currentPercent = 0f;
        float moveDuration = 1f;

        while (currentTime <= 1)
        {
            currentTime += Time.fixedDeltaTime;
            currentPercent = currentTime / moveDuration;

            _rigidbody.position = Vector2.Lerp(startPos, targetPos, currentPercent);

            yield return new WaitForFixedUpdate();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponent<PZ_Soccer_Gate>())
        {
            return;
        }

        // 여기에 축구화(장비:신발)를 주는 로직 구현 예정

        Instantiate(_clearObject, transform.position, transform.rotation);

        _Soccer.ClearPuzzle();
    }
}