using UnityEngine;
using System.Collections;

public class PZ_Soccer_Gate : MonoBehaviour
{
    private enum GateMove
    {
        Left, Right, Up
    }

    [SerializeField] private Rigidbody2D _rigidbody;

    private void Start()
    {
        PZ_Soccer_Ball.KickBallStart += MoveStart;
        PZ_Soccer_Ball.ResetBall += ResetPosition;
    }

    private void MoveStart()
    {
        int randomMove = Random.Range(0, 3);

        switch (randomMove)
        {
            case 0:
                StartCoroutine(MoveGate(GateMove.Up));
                break;

            case 1:
                StartCoroutine(MoveGate(GateMove.Left));
                break;

            case 2:
                StartCoroutine(MoveGate(GateMove.Right));
                break;
        }
    }

    private IEnumerator MoveGate(GateMove gateDir)
    {
        Vector2 startPos = _rigidbody.position; // 시작점
        Vector2 targetPos = _rigidbody.position; // 도착점

        if (gateDir == GateMove.Left)
        {
            targetPos.x -= 2.5f;
        }
        else if (gateDir == GateMove.Right)
        {
            targetPos.x += 2.5f;
        }
        else if (gateDir == GateMove.Up)
        {
            targetPos.y += 2.5f;
        }

        float currentTime = 0f;
        float currentPercent = 0f;
        float moveDuration = 0.5f;

        while (currentTime <= 1)
        {
            currentTime += Time.fixedDeltaTime;
            currentPercent = currentTime / moveDuration;

            _rigidbody.position = Vector2.Lerp(startPos, targetPos, currentPercent);

            yield return new WaitForFixedUpdate();
        }
    }

    private void ResetPosition()
    {
        _rigidbody.position = new Vector2(69.4f, 103.58f);
    }

    private void OnDestroy()
    {
        PZ_Soccer_Ball.KickBallStart -= MoveStart;
        PZ_Soccer_Ball.ResetBall -= ResetPosition;
    }
}