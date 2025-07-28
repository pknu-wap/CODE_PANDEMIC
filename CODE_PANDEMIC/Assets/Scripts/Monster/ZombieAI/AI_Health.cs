using System;
using UnityEngine;

public class AI_Health : MonoBehaviour
{
    private MonsterData _monsterData;
    private int _currentHp;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDied;

    public int CurrentHp => _currentHp;
    public int MaxHp => _monsterData.Hp;
    public MonsterData MonsterData => _monsterData;

    public void SetInfo(MonsterData monsterData)
    {
        _monsterData = monsterData;
        _currentHp = monsterData.Hp;
    }

    public void TakeDamage(int amount)
    {
        if (_currentHp <= 0)
            return;

        _currentHp -= amount;
        _currentHp = Mathf.Max(_currentHp, 0);

        OnHealthChanged?.Invoke(_currentHp, _monsterData.Hp);

        if (_currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDied?.Invoke();
    }
}