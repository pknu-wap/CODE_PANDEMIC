using UnityEngine;
using UnityEngine.UI;

public class PZ_LightOut_Button : UI_Base
{
    private RectTransform _rectTransform;
    private Image _image;

    [SerializeField]
    private Material _correctMaterial; // 맞았을때 색
    [SerializeField]
    private Material _wrongMaterial; // 틀렸을때 색

    private int _buttonIndex; // 현재 버튼 Index
    private bool _isCorrectState = false; // 현재 버튼이 올바른 상태인지 체크

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

    // 현재 버튼의 상태와 그에 맞는 색으로 변경
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

    // 현재 버튼이 올바른 색을 가졌는지를 반환
    public bool IsButtonCorrect()
    {
        return _isCorrectState;
    }

    // 버튼 클릭 이벤트
    private void OnButtonClick()
    {
        Debug.Log("클릭한 버튼 : " + _buttonIndex);

        PZ_LightOut_Board board = GetComponentInParent<PZ_LightOut_Board>();
        board.ChangeButtonsState(_buttonIndex);
        board.CheckButtonsCorrect();
    }
}