using UnityEngine;

public class PZ_Help : UI_Base
{
    [SerializeField] private string _hintText;

    private PZ_Help_Text _text;

    private void Start()
    {
        _text = GetComponentInChildren<PZ_Help_Text>();

        _text.Setting(_hintText);

        BindEvent(gameObject, OnButtonClick, Define.UIEvent.Click);
    }

    public void OnButtonClick()
    {
        _text.ToggleHint();
    }
}