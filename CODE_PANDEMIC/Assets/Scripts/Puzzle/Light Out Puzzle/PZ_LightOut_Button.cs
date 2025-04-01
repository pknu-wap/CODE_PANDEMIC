using UnityEngine;
using UnityEngine.UI;

public class PZ_LightOut_Button : UI_Base
{
    private RectTransform _rectTransform;
    private Image _image;

    private Material _correctMaterial; // 맞았을때 색
    private Material _wrongMaterial; // 틀렸을때 색

    private int _buttonIndex; // 현재 버튼 Index
    private bool _isCorrectState = false; // 현재 버튼이 올바른 상태인지 체크

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

    // 무작위 버튼 상태 설정
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