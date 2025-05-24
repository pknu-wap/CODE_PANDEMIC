using UnityEngine;

public class UI_EnemyStatusBar : MonoBehaviour
{
    [SerializeField] private Transform _hpBarTransform;

    private float _originalScaleX;
    private int _maxHp;
    
    public virtual void Init(MonsterData monsterData)
    {
        _maxHp = monsterData.Hp;
        _originalScaleX = _hpBarTransform.localScale.x;
        UpdateHpBar(monsterData.Hp);
        
    }

    public void UpdateHpBar(int currentHp)
    {
        if (currentHp == 0) gameObject.SetActive(false);
        float ratio = Mathf.Clamp01(currentHp / (float)_maxHp);
        Vector3 scale = _hpBarTransform.localScale;
        scale.x = _originalScaleX * ratio;
        _hpBarTransform.localScale = scale;
    }
}
