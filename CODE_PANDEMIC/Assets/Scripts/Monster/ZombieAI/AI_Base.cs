using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class AI_Base : MonoBehaviour
{
    // 기본 체력, 데미지, 이동 속도, 감지 범위, 시야각 등 공통 속성
    [SerializeField] UI_EnemyStatusBar _statusBar;
    protected MonsterData _monsterData;
    int _currentHp = 0;
    protected AI_State _state = AI_State.Idle;
    public event Action OnDie;
    public void SetInfo(MonsterData monsterData)
    {
        _monsterData = monsterData;
        _currentHp = monsterData.Hp;
        _statusBar.Init(_currentHp);
    }
    public virtual bool Init()
    {
        if (_monsterData == null)
        {
            _monsterData.NameID = "TestZombie";
            _monsterData.Hp = 100;
            _currentHp = _monsterData.Hp;
            _monsterData.AttackDelay = 5.0f;
            _monsterData.DetectionRange = 7.5f;
            _monsterData.DetectionAngle = 120;
            _monsterData.MoveSpeed = 1.0f;
            _monsterData.AttackRange = 2f;
            _monsterData.AttackDamage = 10;

        }
        return true;
    }

    public virtual void SetState(AI_State state)
    {
        _state = state;
    }

    public AI_State GetState()
    {
        return _state;
    }

    public virtual void TakeDamage(int amount)
    {
        _currentHp -= amount;
        if(_statusBar.gameObject.activeSelf==false)
            _statusBar.gameObject.SetActive(true);
        _statusBar?.UpdateHpBar(Mathf.RoundToInt(_currentHp));
        
        if (_currentHp<= 0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        _state = AI_State.Dead;

        Action callback = OnDie;
        if (callback != null)
            callback();

        gameObject.SetActive(false);
    }
    public float MoveSpeed { get { return _monsterData.MoveSpeed; } }
    public float DetectionRange { get { return _monsterData.DetectionRange; } }
    public float DetectionAngle { get { return _monsterData.DetectionAngle; } }
    public float Damage { get { return _monsterData.AttackDamage; } }
    public int Health { get { return _currentHp; } }
    public float AttackRange { get { return _monsterData.AttackRange; } }
    public float AttackDelay { get { return _monsterData.AttackDelay; } }
}