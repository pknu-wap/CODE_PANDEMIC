using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStatusBar : UI_Base
{
    enum Images
    {
        PlayerEffectStatusBar,
        PlayerStatusBar
    }

    private Image _realHpBar;
    private Image _effectHpBar;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));

        _realHpBar = GetImage((int)Images.PlayerStatusBar);
        _effectHpBar = GetImage((int)Images.PlayerEffectStatusBar);

        return true;
    }

    public void UpdateHp(float realRatio, float effectRatio)
    {
        if (_realHpBar != null) _realHpBar.fillAmount = realRatio;
        if (_effectHpBar != null) _effectHpBar.fillAmount = effectRatio;
    }
}
