using UnityEngine;
using System.Collections;

public enum CarMoveDirection
{
    None,
    North,
    West,
    South,
    East
}

public class PZ_Car : PZ_Interact_NonSpawn
{
    [SerializeField] private Rigidbody2D _rigidbody;

    [SerializeField] private bool _isMainCar = false;
    [SerializeField] private bool _isVerticalCar = false;

    public int[] _body1Index = new int[2];
    public int[] _body2Index = new int[2];

    private CarMoveDirection _direction = CarMoveDirection.None;

    private float _moveValue = 3f;

    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        StartCoroutine(MoveCar());
    }

    private IEnumerator CheckDirection()
    {
        yield return new WaitUntil(() =>
        Input.GetKeyDown(KeyCode.W) ||
        Input.GetKeyDown(KeyCode.A) ||
        Input.GetKeyDown(KeyCode.S) ||
        Input.GetKeyDown(KeyCode.D)
        );

        if (Input.GetKeyDown(KeyCode.W))
        {
            _direction = CarMoveDirection.North;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _direction = CarMoveDirection.West;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _direction = CarMoveDirection.South;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _direction = CarMoveDirection.East;
        }
    }

    private IEnumerator MoveCar()
    {
        yield return StartCoroutine(CheckDirection());

        Vector2 currentPos = _rigidbody.position;
        Vector2 destinationPos = _rigidbody.position;

        if (_direction == CarMoveDirection.North)
        {
            destinationPos.y += _moveValue;
        }
        else if (_direction == CarMoveDirection.West)
        {
            destinationPos.x -= _moveValue;
        }
        else if (_direction == CarMoveDirection.South)
        {
            destinationPos.y -= _moveValue;
        }
        else if (_direction == CarMoveDirection.East)
        {
            destinationPos.x += _moveValue;
        }

        float currentTime = 0f;
        float currentPercent = 0f;
        float moveDuration = 0.5f;

        while (currentTime <= 1)
        {
            currentTime += Time.fixedDeltaTime;
            currentPercent = currentTime / moveDuration;

            _rigidbody.position = Vector2.Lerp(currentPos, destinationPos, currentPercent);

            yield return new WaitForFixedUpdate();
        }

        _isInteracted = false;
        _direction = CarMoveDirection.None;
    }
}