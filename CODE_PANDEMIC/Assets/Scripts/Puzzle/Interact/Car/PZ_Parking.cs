using UnityEngine;
using System.Collections.Generic;

public class PZ_Parking : MonoBehaviour
{
    [SerializeField] private GameObject _arrowUp;
    [SerializeField] private GameObject _arrowLeft;
    [SerializeField] private GameObject _arrowDown;
    [SerializeField] private GameObject _arrowRight;

    private List<GameObject> _arrowList = new List<GameObject>();

    private bool[,] _emptyPlace = new bool[6, 6]; // 빈 장소 목록

    // 정답이 정해진 퍼즐이므로 빈 장소 하드 코딩
    private void Start()
    {
        _emptyPlace[2, 0] = true;

        _emptyPlace[4, 1] = true;

        _emptyPlace[3, 2] = true;

        _emptyPlace[1, 4] = true;

        _emptyPlace[3, 5] = true;
        _emptyPlace[5, 5] = true;
    }

    // 해당 방향으로 차를 움직일 수 있는 지 체크
    public bool CanMoveCar(PZ_Car moveCar, CarMoveDirection direction, bool isVerticalCar, int[] body1Index, int[] body2Index)
    {
        if (isVerticalCar) // 세로로 배치된 차의 경우
        {
            switch (direction)
            {
                case CarMoveDirection.North: // 위로 움직이는 경우

                    if (body1Index[0] == 0 || !IsEmptyPlace(body1Index[0] - 1, body1Index[1]))
                    {
                        return false;
                    }

                    _emptyPlace[body1Index[0] - 1, body1Index[1]] = false;
                    _emptyPlace[body2Index[0], body2Index[1]] = true;

                    moveCar.SetBodyIndex(body1Index[0] - 1, body1Index[1], body2Index[0] - 1, body2Index[1]);

                    return true;

                case CarMoveDirection.West: // 왼쪽으로 움직이는 경우

                    if (body1Index[1] == 0 || !IsEmptyPlace(body1Index[0], body1Index[1] - 1) || !IsEmptyPlace(body2Index[0], body2Index[1] - 1))
                    {
                        return false;
                    }

                    _emptyPlace[body1Index[0], body1Index[1] - 1] = false;
                    _emptyPlace[body1Index[0], body1Index[1]] = true;
                    _emptyPlace[body2Index[0], body2Index[1] - 1] = false;
                    _emptyPlace[body2Index[0], body2Index[1]] = true;

                    moveCar.SetBodyIndex(body1Index[0], body1Index[1] - 1, body2Index[0], body2Index[1] - 1);

                    return true;

                case CarMoveDirection.South: // 아래로 움직이는 경우

                    if (body2Index[0] == 5 || !IsEmptyPlace(body2Index[0] + 1, body2Index[1]))
                    {
                        return false;
                    }

                    _emptyPlace[body2Index[0] + 1, body2Index[1]] = false;
                    _emptyPlace[body1Index[0], body1Index[1]] = true;

                    moveCar.SetBodyIndex(body1Index[0] + 1, body1Index[1], body2Index[0] + 1, body2Index[1]);

                    return true;

                case CarMoveDirection.East: // 오른쪽으로 움직이는 경우

                    if (body1Index[1] == 5 || !IsEmptyPlace(body1Index[0], body1Index[1] + 1) || !IsEmptyPlace(body2Index[0], body2Index[1] + 1))
                    {
                        return false;
                    }

                    _emptyPlace[body1Index[0], body1Index[1] + 1] = false;
                    _emptyPlace[body1Index[0], body1Index[1]] = true;
                    _emptyPlace[body2Index[0], body2Index[1] + 1] = false;
                    _emptyPlace[body2Index[0], body2Index[1]] = true;

                    moveCar.SetBodyIndex(body1Index[0], body1Index[1] + 1, body2Index[0], body2Index[1] + 1);

                    return true;
            }
        }

        else // 가로로 배치된 차의 경우
        {
            switch (direction)
            {
                case CarMoveDirection.North: // 위로 움직이는 경우

                    if (body1Index[0] == 0 || !IsEmptyPlace(body1Index[0] - 1, body1Index[1]) || !IsEmptyPlace(body2Index[0] - 1, body2Index[1]))
                    {
                        return false;
                    }

                    _emptyPlace[body1Index[0] - 1, body1Index[1]] = false;
                    _emptyPlace[body1Index[0], body1Index[1]] = true;
                    _emptyPlace[body2Index[0] - 1, body2Index[1]] = false;
                    _emptyPlace[body2Index[0], body2Index[1]] = true;

                    moveCar.SetBodyIndex(body1Index[0] - 1, body1Index[1], body2Index[0] - 1, body2Index[1]);

                    return true;

                case CarMoveDirection.West: // 왼쪽으로 움직이는 경우

                    if (body1Index[1] == 0 || !IsEmptyPlace(body1Index[0], body1Index[1] - 1))
                    {
                        return false;
                    }

                    _emptyPlace[body1Index[0], body1Index[1] - 1] = false;
                    _emptyPlace[body2Index[0], body2Index[1]] = true;

                    moveCar.SetBodyIndex(body1Index[0], body1Index[1] - 1, body2Index[0], body2Index[1] - 1);

                    return true;

                case CarMoveDirection.South: // 아래로 움직이는 경우

                    if (body1Index[0] == 5 || !IsEmptyPlace(body1Index[0] + 1, body1Index[1]) || !IsEmptyPlace(body2Index[0] + 1, body2Index[1]))
                    {
                        return false;
                    }

                    _emptyPlace[body1Index[0] + 1, body1Index[1]] = false;
                    _emptyPlace[body1Index[0], body1Index[1]] = true;
                    _emptyPlace[body2Index[0] + 1, body2Index[1]] = false;
                    _emptyPlace[body2Index[0], body2Index[1]] = true;

                    moveCar.SetBodyIndex(body1Index[0] + 1, body1Index[1], body2Index[0] + 1, body2Index[1]);

                    return true;

                case CarMoveDirection.East: // 오른쪽으로 움직이는 경우

                    if (body2Index[1] == 5 || !IsEmptyPlace(body2Index[0], body2Index[1] + 1))
                    {
                        return false;
                    }

                    _emptyPlace[body2Index[0], body2Index[1] + 1] = false;
                    _emptyPlace[body1Index[0], body1Index[1]] = true;

                    moveCar.SetBodyIndex(body1Index[0], body1Index[1] + 1, body2Index[0], body2Index[1] + 1);

                    return true;
            }
        }

        return false;
    }

    // 해당 위치가 빈 자리인지 체크
    private bool IsEmptyPlace(int index1, int index2)
    {
        if (_emptyPlace[index1, index2])
        {
            return true;
        }

        return false;
    }

    public void SpawnArrows(Transform carTransform, CarMoveDirection direction, bool isVerticalCar, int[] body1Index, int[] body2Index)
    {
        if (isVerticalCar) // 세로로 배치된 차의 경우
        {
            switch (direction)
            {
                case CarMoveDirection.North: // 위로 움직이는 경우

                    if (body1Index[0] != 0 && IsEmptyPlace(body1Index[0] - 1, body1Index[1]))
                    {
                        Vector3 targetPos = carTransform.position;
                        targetPos.y += 0.5f;
                        _arrowList.Add(Instantiate(_arrowUp, targetPos, new Quaternion(0, 0, 0, 0)));
                    }

                    break;

                case CarMoveDirection.West: // 왼쪽으로 움직이는 경우

                    if (body1Index[1] != 0 && IsEmptyPlace(body1Index[0], body1Index[1] - 1) && IsEmptyPlace(body2Index[0], body2Index[1] - 1))
                    {
                        Vector3 targetPos = carTransform.position;
                        targetPos.x -= 0.5f;
                        _arrowList.Add(Instantiate(_arrowLeft, targetPos, new Quaternion(0, 0, 0, 0)));
                    }

                    break;

                case CarMoveDirection.South: // 아래로 움직이는 경우

                    if (body2Index[0] != 5 && IsEmptyPlace(body2Index[0] + 1, body2Index[1]))
                    {
                        Vector3 targetPos = carTransform.position;
                        targetPos.y -= 0.5f;
                        _arrowList.Add(Instantiate(_arrowDown, targetPos, new Quaternion(0, 0, 0, 0)));
                    }

                    break;

                case CarMoveDirection.East: // 오른쪽으로 움직이는 경우

                    if (body1Index[1] != 5 && IsEmptyPlace(body1Index[0], body1Index[1] + 1) && IsEmptyPlace(body2Index[0], body2Index[1] + 1))
                    {
                        Vector3 targetPos = carTransform.position;
                        targetPos.x += 0.5f;
                        _arrowList.Add(Instantiate(_arrowRight, targetPos, new Quaternion(0, 0, 0, 0)));
                    }

                    break;
            }
        }

        else // 가로로 배치된 차의 경우
        {
            switch (direction)
            {
                case CarMoveDirection.North: // 위로 움직이는 경우

                    if (body1Index[0] != 0 && IsEmptyPlace(body1Index[0] - 1, body1Index[1]) && IsEmptyPlace(body2Index[0] - 1, body2Index[1]))
                    {
                        Vector3 targetPos = carTransform.position;
                        targetPos.y += 0.5f;
                        _arrowList.Add(Instantiate(_arrowUp, targetPos, new Quaternion(0, 0, 0, 0)));
                    }

                    break;

                case CarMoveDirection.West: // 왼쪽으로 움직이는 경우

                    if (body1Index[1] != 0 && IsEmptyPlace(body1Index[0], body1Index[1] - 1))
                    {
                        Vector3 targetPos = carTransform.position;
                        targetPos.x -= 0.5f;
                        _arrowList.Add(Instantiate(_arrowLeft, targetPos, new Quaternion(0, 0, 0, 0)));
                    }

                    break;

                case CarMoveDirection.South: // 아래로 움직이는 경우

                    if (body1Index[0] != 5 && IsEmptyPlace(body1Index[0] + 1, body1Index[1]) && IsEmptyPlace(body2Index[0] + 1, body2Index[1]))
                    {
                        Vector3 targetPos = carTransform.position;
                        targetPos.y -= 0.5f;
                        _arrowList.Add(Instantiate(_arrowDown, targetPos, new Quaternion(0, 0, 0, 0)));
                    }

                    break;

                case CarMoveDirection.East: // 오른쪽으로 움직이는 경우

                    if (body2Index[1] != 5 && IsEmptyPlace(body2Index[0], body2Index[1] + 1))
                    {
                        Vector3 targetPos = carTransform.position;
                        targetPos.x += 0.5f;
                        _arrowList.Add(Instantiate(_arrowRight, targetPos, new Quaternion(0,0,0,0)));
                    }

                    break;
            }
        }
    }

    public void DestroyAllArrows()
    {
        for (int index = 0; index < _arrowList.Count; index++)
        {
            Destroy(_arrowList[index]);
        }

        _arrowList.Clear();
    }
}