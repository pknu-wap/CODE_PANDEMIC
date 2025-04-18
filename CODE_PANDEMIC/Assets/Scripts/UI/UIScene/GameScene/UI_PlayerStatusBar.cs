using UnityEngine;

public class UI_PlayerStatusBar : UI_Base
{
    enum Images
    {
        PlayerEffectStatusBar,
        PlayerStatusBar
    }

    private RectTransform _realHpBar;
    private RectTransform _effectHpBar;

    private float _originWidth;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));

        _realHpBar = GetImage((int)Images.PlayerStatusBar).GetComponent<RectTransform>();
        _effectHpBar = GetImage((int)Images.PlayerEffectStatusBar).GetComponent<RectTransform>();

        _originWidth = _realHpBar.sizeDelta.x;

        return true;
    }

    public void UpdateHp(float realRatio, float effectRatio)
    {
     

        if (_realHpBar != null)
        {
            Vector2 size = _realHpBar.sizeDelta;
            size.x = _originWidth * Mathf.Clamp01(realRatio);
            _realHpBar.sizeDelta = size;
        }

        if (_effectHpBar != null)
        {
            Vector2 size = _effectHpBar.sizeDelta;
            size.x = _originWidth * Mathf.Clamp01(effectRatio);
            _effectHpBar.sizeDelta = size;
        }
    }
}
