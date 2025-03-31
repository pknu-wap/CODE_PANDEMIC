using UnityEngine;
using UnityEngine.UI;

public class PZ_LightOut_Button : UI_Base
{
    private RectTransform _rectTransform;
    private Image _image;

    [SerializeField]
    private Material _correctMaterial; // �¾����� ��
    [SerializeField]
    private Material _wrongMaterial; // Ʋ������ ��

    private int _buttonIndex; // ���� ��ư Index
    private bool _isCorrectState = false; // ���� ��ư�� �ùٸ� �������� üũ

    public void Init(int buttonIndex)
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();

        _image.material = _wrongMaterial;
        _image.SetMaterialDirty();

        _buttonIndex = buttonIndex;

        _rectTransform.sizeDelta = new Vector2(160, 160);

        BindEvent(gameObject, OnButtonClick);
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