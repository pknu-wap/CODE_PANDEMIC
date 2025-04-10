using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PZ_LightOut_Board : PZ_Puzzle_Base
{
    #region Base

    private GridLayoutGroup _gridLayoutGroup;

    private List<PZ_LightOut_Button> _lightOutButtonList = new List<PZ_LightOut_Button>(); // ��ȯ�� ��ư�� ����
    private PZ_LightOut_Reset _resetButton;

    private int _buttonMaxCount = 25; // Light ��ư ����

    public override bool Init()
    {
        SetComponents();

        _gridLayoutGroup = GetComponent<GridLayoutGroup>();

        Managers.Resource.Instantiate("PZ_LightOut_Reset_Prefab", GetComponentInParent<Canvas>().transform, (resetButton) =>
        {
            _resetButton = resetButton.GetComponent<PZ_LightOut_Reset>();
        });

        // ���� �⺻ ����
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = new Vector2(0, 0);
        _rectTransform.sizeDelta = new Vector2(900, 900);

        // �̹��� ����
        Managers.Resource.LoadAsync<Sprite>("PZ_LightOut_Board_Sprite", (getSprite) =>
        {
            _image.sprite = getSprite;
        });

        // ������ ��ư�� ��ġ ����
        _gridLayoutGroup.padding.left = 10;
        _gridLayoutGroup.padding.right = 10;
        _gridLayoutGroup.padding.top = 10;
        _gridLayoutGroup.padding.bottom = 10;
        _gridLayoutGroup.cellSize = new Vector2(160, 160);
        _gridLayoutGroup.spacing = new Vector2(10, 10);

        GetSpawnedButtons();

        return true;
    }

    private void OnDestroy()
    {
        if (_resetButton)
        {
            Destroy(_resetButton.gameObject);
        }
    }

    #endregion

    #region Setting

    // Light Button ��������
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

    #endregion

    #region Click

    // ����
    public void ResetButtons()
    {
        for (int index = 0; index < _buttonMaxCount; index++)
        {
            _lightOutButtonList[index].ShuffleButtonState();
        }
    }

    // Ŭ���� ��ư�� �����¿��� ��ư���� ���¸� �����Ű�� �Լ�
    public void ChangeButtonsState(int currentIndex)
    {
        // ���� ��ư ��ġ ���� ����
        _lightOutButtonList[currentIndex].ChangeButtonState();

        // �� ���� ��ȿ üũ
        if (currentIndex - 5 >= 0)
        {
            _lightOutButtonList[currentIndex - 5].ChangeButtonState();
        }

        // �� ���� ��ȿ üũ
        if (currentIndex + 5 <= _buttonMaxCount - 1)
        {
            _lightOutButtonList[currentIndex + 5].ChangeButtonState();
        }

        // �� ���� ��ȿ üũ
        if (currentIndex != 0 && currentIndex != 5 && currentIndex != 10 && currentIndex != 15 && currentIndex != 20)
        {
            _lightOutButtonList[currentIndex - 1].ChangeButtonState();
        }

        // �� ���� ��ȿ üũ
        if (currentIndex != 4 && currentIndex != 9 && currentIndex != 14 && currentIndex != 19 && currentIndex != 24)
        {
            _lightOutButtonList[currentIndex + 1].ChangeButtonState();
        }
    }

    #endregion

    #region Clear

    // ��ư���� ���� �ùٸ� �������� üũ
    public void CheckButtonsCorrect()
    {
        if (_lightOutButtonList.FindAll(button => button.IsButtonCorrect() == true).Count == _buttonMaxCount)
        {
            PuzzleClear();
        }
    }

    // ���� Ŭ����
    protected override void PuzzleClear()
    {
        Debug.LogWarning("Light Out Puzzle Clear!!!");

        _puzzleOwner.ClearPuzzle();
    }

    #endregion
}