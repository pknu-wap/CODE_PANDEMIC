using UnityEngine;
using UnityEngine.UI;

public class PZ_LightOut_Button : UI_Base
{
    private RectTransform _rectTransform;
    private Image _image;

    private Sprite _correctSprite; // �¾����� ��
    private Sprite _wrongSprite; // Ʋ������ ��

    private int _buttonIndex; // ���� ��ư Index
    private bool _isCorrectState = false; // ���� ��ư�� �ùٸ� �������� üũ

    public void Init(int buttonIndex)
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();

        Managers.Resource.LoadAsync<Sprite>("PZ_LightOut_Correct_Sprite", (getSprite) =>
        {
            _correctSprite = getSprite;
        });

        Managers.Resource.LoadAsync<Sprite>("PZ_LightOut_Wrong_Sprite", (getSprite) =>
        {
            _wrongSprite = getSprite;
            ShuffleButtonState();
        });

        _buttonIndex = buttonIndex;

        _rectTransform.sizeDelta = new Vector2(160, 160);

        BindEvent(gameObject, OnButtonClick);
    }

    // ������ ��ư ���� ����
    public void ShuffleButtonState()
    {
        int randomState = Random.Range(0, 2);

        if (randomState == 0)
        {
            _image.sprite = _correctSprite;
            _isCorrectState = true;
        }
        else
        {
            _image.sprite = _wrongSprite;
            _isCorrectState = false;
        }
    }

    // ���� ��ư�� ���¿� �׿� �´� ������ ����
    public void ChangeButtonState()
    {
        if (_isCorrectState)
        {
            _image.sprite = _wrongSprite;
        }
        else
        {
            _image.sprite = _correctSprite;
        }

        _image.canvasRenderer.SetMaterial(_image.material, null);
        _isCorrectState = !_isCorrectState;
    }

    // ���� ��ư�� �ùٸ� ���� ���������� ��ȯ
    public bool IsButtonCorrect()
    {
        return _isCorrectState;
    }

    // ��ư Ŭ�� �̺�Ʈ
    private void OnButtonClick()
    {
        Debug.Log("Ŭ���� ��ư : " + _buttonIndex);

        PZ_LightOut_Board board = GetComponentInParent<PZ_LightOut_Board>();
        board.ChangeButtonsState(_buttonIndex);
        board.CheckButtonsCorrect();
    }
}