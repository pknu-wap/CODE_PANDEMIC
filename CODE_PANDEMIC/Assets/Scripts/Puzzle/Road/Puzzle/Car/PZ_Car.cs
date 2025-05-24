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
    private PZ_Parking _Parking;
    private Rigidbody2D _rigidbody;

    [SerializeField] private bool _isMainCar = false; // 꺼내야 하는 차인가
    [SerializeField] private bool _isVerticalCar = false; // 세로로 배치된 차인가

    public int[] _body1Index = new int[2]; // 첫번째 몸통 인덱스 (가로의 경우 왼쪽, 세로의 경우 위쪽)
    public int[] _body2Index = new int[2]; // 두번째 몸통 인덱스 (가로의 경우 오른쪽, 세로의 경우 아래쪽)

    private CarMoveDirection _direction = CarMoveDirection.None;

    private float _moveValue = 3f; // 이동 거리

    [SerializeField] private Sprite _upSprite;

    private void Start()
    {
        _Parking = GetComponentInParent<PZ_Parking>();
        _rigidbody = GetComponentInParent<Rigidbody2D>();
    }

    public void SetBodyIndex(int body1_x, int body1_y, int body2_x, int body2_y)
    {
        _body1Index[0] = body1_x;
        _body1Index[1] = body1_y;
        _body2Index[0] = body2_x;
        _body2Index[1] = body2_y;
    }

    // 상호 작용
    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        StartCoroutine(MoveCar());
    }

    // 방향키 입력 대기
    private IEnumerator CheckDirection()
    {
        _Parking.SpawnArrows(transform, CarMoveDirection.North, _isVerticalCar, _body1Index, _body2Index);
        _Parking.SpawnArrows(transform, CarMoveDirection.West, _isVerticalCar, _body1Index, _body2Index);
        _Parking.SpawnArrows(transform, CarMoveDirection.South, _isVerticalCar, _body1Index, _body2Index);
        _Parking.SpawnArrows(transform, CarMoveDirection.East, _isVerticalCar, _body1Index, _body2Index);

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

    // 차 움직이기
    private IEnumerator MoveCar()
    {
        yield return StartCoroutine(CheckDirection()); // 입력 대기

        _Parking.DestroyAllArrows();

        // 움직일 수 있는지 조건 확인
        if (!_Parking.CanMoveCar(this, _direction, _isVerticalCar, _body1Index, _body2Index))
        {
            _isInteracted = false;
            _direction = CarMoveDirection.None;

            yield break;
        }

        Vector2 currentPos = _rigidbody.position; // 시작점
        Vector2 destinationPos = _rigidbody.position; // 도착점

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

        if (_isMainCar)
        {
            CheckPuzzleClear();
        }
    }

    // 클리어 체크
    private void CheckPuzzleClear()
    {
        if (_body1Index[0] == 2 && _body1Index[1] == 4 && _body2Index[0] == 2 && _body2Index[1] == 5)
        {
            Debug.Log("Car Puzzle Clear!!!");

            StartCoroutine(RushToBlockObject());

            _Parking.ClearPuzzle();
        }
    }

    // 길막 오브젝트에게 달려가 붐
    private IEnumerator RushToBlockObject()
    {
        Managers.Event.InvokeEvent("Cinematic", Define.CinematicType.PuzzleClear);
        yield return CoroutineHelper.WaitForSeconds(1.0f); //wait for  camera 
        Vector2 currentPos = _rigidbody.position; // 시작점
        Vector2 destinationPos = _rigidbody.position; // 도착점

        destinationPos.x += 8;

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

        _spriteRenderer.sprite = _upSprite;

        currentPos = _rigidbody.position;
        destinationPos = _rigidbody.position;

        destinationPos.y += 210;

        currentTime = 0f;
        currentPercent = 0f;
        moveDuration = 4f;

        while (currentTime <= 1)
        {
            currentTime += Time.fixedDeltaTime;
            currentPercent = currentTime / moveDuration;

            _rigidbody.position = Vector2.Lerp(currentPos, destinationPos, currentPercent);

            yield return new WaitForFixedUpdate();
        }

        // 여기에 폭발 이펙트 및 길막는 오브젝트 파괴 구현
        Managers.Event.InvokeEvent("EndCinematic", Define.CinematicType.PuzzleClear);
        yield return CoroutineHelper.WaitForSeconds(1.0f);

        Destroy(gameObject);
    }
}