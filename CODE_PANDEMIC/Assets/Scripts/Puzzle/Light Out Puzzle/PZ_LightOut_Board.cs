using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PZ_LightOut_Board : UI_PopUp
{
    private RectTransform _rectTransform;
    private Image _image;
    private GridLayoutGroup _gridLayoutGroup;

    private List<PZ_LightOut_Button> _lightOutButtonList = new List<PZ_LightOut_Button>(); // 소환된 버튼들 관리

    private int _buttonMaxCount = 25; // Light 버튼 개수

    public override bool Init()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();

        Managers.Resource.Instantiate("PZ_LightOut_Reset_Prefab", GetComponentInParent<Canvas>().transform);

        // 보드 기본 세팅
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = new Vector2(0, 0);
        _rectTransform.sizeDelta = new Vector2(900, 900);

        // 이미지 세팅
        Managers.Resource.LoadAsync<Sprite>("PZ_LightOut_Board_Sprite", (getSprite) =>
        {
            _image.sprite = getSprite;
        });

        // 스폰될 버튼의 위치 세팅
        _gridLayoutGroup.padding.left = 10;
        _gridLayoutGroup.padding.right = 10;
        _gridLayoutGroup.padding.top = 10;
        _gridLayoutGroup.padding.bottom = 10;
        _gridLayoutGroup.cellSize = new Vector2(160, 160);
        _gridLayoutGroup.spacing = new Vector2(10, 10);

        return true;
    }

    private void Start()
    {
        if (!Init())
        {
            return;
        }

        GetSpawnedButtons();
    }

    // Light Button 가져오기
    private void GetSpawnedButtons()
    {
        for (int index = 0; index < _buttonMaxCount; index++)
        {
            Transform childButton = transform.GetChild(index);
            PZ_LightOut_Button spawnedButton = childButton.gameObject.GetComponent<PZ_LightOut_Button>();

            spawnedButton.Init(index);

            _lightOutButtonList.Add(spawnedButton);
        }
    }

    // 리셋
    public void ResetButtons()
    {
        for (int index = 0; index < _buttonMaxCount; index++)
        {
            _lightOutButtonList[index].ShuffleButtonState();
        }
    }

    // 클릭한 버튼과 상하좌우의 버튼들의 상태를 변경시키는 함수
    public void ChangeButtonsState(int currentIndex)
    {
        // 현재 버튼 위치 상태 변경
        _lightOutButtonList[currentIndex].ChangeButtonState();

        // 상 방향 유효 체크
        if (currentIndex - 5 >= 0)
        {
            _lightOutButtonList[currentIndex - 5].ChangeButtonState();
        }

        // 하 방향 유효 체크
        if (currentIndex + 5 <= _buttonMaxCount - 1)
        {
            _lightOutButtonList[currentIndex + 5].ChangeButtonState();
        }

        // 좌 방향 유효 체크
        if (currentIndex != 0 && currentIndex != 5 && currentIndex != 10 && currentIndex != 15 && currentIndex != 20)
        {
            _lightOutButtonList[currentIndex - 1].ChangeButtonState();
        }

        // 우 방향 유효 체크
        if (currentIndex != 4 && currentIndex != 9 && currentIndex != 14 && currentIndex != 19 && currentIndex != 24)
        {
            _lightOutButtonList[currentIndex + 1].ChangeButtonState();
        }
    }

    // 버튼들이 전부 올바른 상태인지 체크
    public void CheckButtonsCorrect()
    {
        if (_lightOutButtonList.FindAll(button => button.IsButtonCorrect() == true).Count == _buttonMaxCount)
        {
            PuzzleClear();
        }
    }

    // 퍼즐 클리어
    private void PuzzleClear()
    {
        Debug.LogWarning("Light Out Puzzle Clear!!!");
        // 여기에 클리어 로직 구현
    }
}