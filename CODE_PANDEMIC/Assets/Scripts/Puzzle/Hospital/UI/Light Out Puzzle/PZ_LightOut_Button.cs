using UnityEngine;
using UnityEngine.UI;

public class PZ_LightOut_Button : UI_Base
{
    private Image _image;

    private Sprite _correctSprite;
    private Sprite _wrongSprite;

    private int _buttonIndex;
    private bool _isCorrectState = false;

    public void Setting(int buttonIndex)
    {
        _image = GetComponent<Image>();

        Managers.Resource.LoadAsync<Sprite>("PZ_LightOut_Correct_Sprite", (getSprite) =>
        {
            _correctSprite = getSprite;

            Managers.Resource.LoadAsync<Sprite>("PZ_LightOut_Wrong_Sprite", (getSprite) =>
            {
                _wrongSprite = getSprite;

                ShuffleButtonState();
            });
        });

        _buttonIndex = buttonIndex;

        BindEvent(gameObject, OnButtonClick, Define.UIEvent.Click);
    }

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

    public bool IsButtonCorrect()
    {
        return _isCorrectState;
    }

    private void OnButtonClick()
    {
        PZ_LightOut_Board board = GetComponentInParent<PZ_LightOut_Board>();
        board.ChangeButtonsState(_buttonIndex);
        board.CheckButtonsCorrect();
    }
}