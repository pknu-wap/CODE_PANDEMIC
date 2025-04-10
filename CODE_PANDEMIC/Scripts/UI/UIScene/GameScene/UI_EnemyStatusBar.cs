using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EnemyStatusBar : MonoBehaviour
{
    [SerializeField] private RectTransform _hpBar;

    private float _originalWidth;
    private int _maxHp;

    public void Init(int maxHp)
    {
        _maxHp = maxHp;
        _originalWidth = _hpBar.sizeDelta.x;
        UpdateHpBar(maxHp);
    }

    public void UpdateHpBar(int currentHp)
    {
        float ratio = Mathf.Clamp01(currentHp / (float)_maxHp);
        _hpBar.sizeDelta = new Vector2(_originalWidth * ratio, _hpBar.sizeDelta.y);
    }
}
