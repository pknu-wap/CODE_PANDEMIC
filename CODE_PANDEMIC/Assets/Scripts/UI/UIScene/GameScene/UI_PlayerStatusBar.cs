using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStatusBar : UI_Base
{
    enum Images { PlayerStatusBar }

    [SerializeField]
    private RectTransform _hpBarTransform;
    [SerializeField]
    private float _originalWidth;

    private int _maxHp = 0;
    private Coroutine _hpAnimRoutine;

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
        OnMaxHpSet += SetInfo;
        OnHpUpdated += AnimateHpBar;
    }

    private void OnDestroy()
    {
        OnMaxHpSet -= SetInfo;
        OnHpUpdated -= AnimateHpBar;
    }

    private void SetInfo(int hp)
    {
        _maxHp = hp;
        SetHpInstant(hp); 
    }

    private void SetHpInstant(int currentHp)
    {
        if (_maxHp <= 0) return;
        float ratio = Mathf.Clamp(currentHp / (float)_maxHp, 0f, 1f);
        _hpBarTransform.sizeDelta = new Vector2(_originalWidth * ratio, _hpBarTransform.sizeDelta.y);
    }

    private void AnimateHpBar(int currentHp)
    {
        if (_maxHp <= 0) return;

        if (_hpAnimRoutine != null)
            StopCoroutine(_hpAnimRoutine);

        _hpAnimRoutine = StartCoroutine(AnimateBar(currentHp));
    }

    private IEnumerator AnimateBar(int currentHp)
    {
        float targetRatio = Mathf.Clamp(currentHp / (float)_maxHp, 0f, 1f);
        float currentRatio = _hpBarTransform.sizeDelta.x / _originalWidth;

        const float speed = 4f; 

        while (!Mathf.Approximately(currentRatio, targetRatio))
        {
            currentRatio = Mathf.MoveTowards(currentRatio, targetRatio, Time.deltaTime * speed);
            _hpBarTransform.sizeDelta = new Vector2(_originalWidth * currentRatio, _hpBarTransform.sizeDelta.y);
            yield return null;
        }

   
        _hpBarTransform.sizeDelta = new Vector2(_originalWidth * targetRatio, _hpBarTransform.sizeDelta.y);
        _hpAnimRoutine = null;
    }
   
}
