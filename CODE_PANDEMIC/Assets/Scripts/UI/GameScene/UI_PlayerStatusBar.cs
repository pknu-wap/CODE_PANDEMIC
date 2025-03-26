using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStatusBar : UI_Base
{
    enum Images { PlayerStatusBar }

    [SerializeField]
    private RectTransform _hpBarTransform;
    private int _maxHp = 0;
    [SerializeField]
    private float _originalWidth;


    //  체력 관련 이벤트만 `Action<int>` 사용
    public static event Action<int> OnMaxHpSet;
    public static event Action<int> OnHpUpdated;

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindImage(typeof(Images));
        _hpBarTransform = GetImage((int)Images.PlayerStatusBar).GetComponent<RectTransform>();
        _originalWidth = _hpBarTransform.sizeDelta.x; 

        return true;
    }

    private void OnEnable()
    {
        OnMaxHpSet += SetMaxHp;
        OnHpUpdated += UpdateHpBar;
    }

    private void OnDisable()
    {
        OnMaxHpSet -= SetMaxHp;
        OnHpUpdated -= UpdateHpBar;
    }

    private void SetMaxHp(int hp)
    {
        _maxHp = hp;
        UpdateHpBar(hp); // 초기 HP 바 설정
    }

    private void UpdateHpBar(int currentHp)
    {
        if (_maxHp <= 0) return;
        float ratio = Mathf.Clamp(currentHp / (float)_maxHp, 0f, 1f);
        _hpBarTransform.sizeDelta = new Vector2(_originalWidth * ratio, _hpBarTransform.sizeDelta.y);
    }
}
