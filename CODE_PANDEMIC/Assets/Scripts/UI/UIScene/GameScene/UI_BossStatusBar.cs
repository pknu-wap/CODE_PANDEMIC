using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BossStatusBar : UI_Base
{
    enum Images
    {
        BossStatusBar
    }
    enum Texts
    {
        BossStatusBar
    }
    [SerializeField]
    private RectTransform _hpBarTransform;
    [SerializeField]
    private float _originalWidth;

    private int _maxHp;
    private int _currentHp;
    public void SetInfo(int id)
    {
        //TODO: DATA
    }
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        _hpBarTransform = GetImage((int)Images.BossStatusBar).rectTransform;

        return true;
    }
   
    
}
