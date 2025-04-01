using UnityEngine;
using UnityEngine.UI;

public class PZ_LightOut_Button : UI_Base
{
    private RectTransform _rectTransform;
    private Image _image;

    private Material _correctMaterial; // �¾����� ��
    private Material _wrongMaterial; // Ʋ������ ��

    private int _buttonIndex; // ���� ��ư Index
    private bool _isCorrectState = false; // ���� ��ư�� �ùٸ� �������� üũ

    public void Init(int buttonIndex)
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();

        Managers.Resource.LoadAsync<Material>("PZ_LightOut_Correct", (getMaterial) =>
        {
            _correctMaterial = getMaterial;
        });

        Managers.Resource.LoadAsync<Material>("PZ_LightOut_Wrong", (getMaterial) =>
        {
            _wrongMaterial = getMaterial;
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
            _image.material = _correctMaterial;
            _isCorrectState = true;
        }
        else
        {
            _image.material = _wrongMaterial;
            _isCorrectState = false;
        }
    }

    // ���� ��ư�� ���¿� �׿� �´� ������ ����
    public void ChangeButtonState()
    {
        if (_isCorrectState)
        {
            _image.material = new Material(_wrongMaterial);
        }
        else
        {
            _image.material = new Material(_correctMaterial);
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