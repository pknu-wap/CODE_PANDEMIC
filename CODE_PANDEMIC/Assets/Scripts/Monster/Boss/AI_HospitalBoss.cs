using System;
using System.Collections;
using UnityEngine;

public class AI_HospitalBoss : AI_Controller
{
    [SerializeField] 
    private CinematicCamera _camera;
    public LayerMask TargetLayer;
    public bool IsBerserk { get; private set; }
    public GameObject _syringePrefab;
    public ThrowSkillData _throwSkillData;
    public SweepSkillData _sweepSkillData;
    public DashSkillData _dashSkillData;
    private bool _shortAttack;

    public AI_LineVisualizer _syringeVisualizer;
    public override float AiDamage => _monsterData.AttackDamage;
    public int MaxHealth => _monsterData.Hp;

    private AI_BossThrow _throwSkill = new AI_BossThrow();
    private AI_BossSweep _sweepSkill = new AI_BossSweep();
    private AI_BossDash _dashSkill = new AI_BossDash();


    public override ISkillBehavior Skill
    {
        get
        {
            if (_dashSkill != null && _dashSkill.IsReady(this) && IsBerserk == true)
            {
                _shortAttack = false;
                return _dashSkill;
            }
            if (_throwSkill != null && _throwSkill.IsReady(this))
            {
                _shortAttack = false;
                return _throwSkill;
            }
                

            if (_sweepSkill != null && _sweepSkill.IsReady(this))
            {
                _shortAttack = true;
                return _sweepSkill;
            }
            
            return null;
        }
    }
   
    private IEnumerator BossSequence()
    {
        if (_camera == null) yield break;
        _camera.gameObject.SetActive(true);
        _camera.OnCinematic();
        //TODO: BOSSSEQUENCE
        yield return CoroutineHelper.WaitForSeconds(3.0f);
        _camera.OnEndCinematic(Define.CinematicType.BossSequence);
    }
    public override void SetInfo(MonsterData monsterData)
    {
        base.SetInfo(monsterData);
        StartCoroutine(BossSequence());
    }
    protected override void Start()
    {
        ChangeState(new AI_StateIdle(this));
        _throwSkill.SetController(this);
        _sweepSkill.SetController(this);
        _dashSkill.SetController(this);
       
        SettingData();
        base.Start();
        if (!Init())
        {
            enabled = false;
            return;
        }
        _sweepSkill.SetSettings(_sweepSkillData, TargetLayer, this);
        _throwSkill.SetSettings(_throwSkillData, TargetLayer, this);
        _dashSkill.SetSettings(_dashSkillData, TargetLayer, this);
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (!IsBerserk && Health <= MaxHealth * 0.5f)
        {
            EnterBerserkMode();
        }
        if (Health <= 0f && _currentState is not AI_StateDie)
        {
            StartCoroutine(BossDeathSequence()); 
           
           
        }
    }

    IEnumerator BossDeathSequence()
    {
        Managers.Event.InvokeEvent("OnBossClear");
        yield return CoroutineHelper.WaitForSeconds(2);
        ChangeState(new AI_StateDie(this));
    }
    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();
    }
  
    private void EnterBerserkMode()
    {
        IsBerserk = true;
    }

    public override bool IsPlayerInSkillRange()
    {
        float distance = Vector2.Distance(transform.position, _player.position);

        if (_sweepSkill != null && _sweepSkill.IsReady(this) && _shortAttack == true)
            return distance <= _sweepSkillData.Range * 0.7f;
        else
            return IsPlayerDetected();
    }
    private void SettingData()
    {
        _sweepSkillData = new SweepSkillData
        {
            Cooldown = 15f,
            Range = 4f,
            Angle = 120f,
            Count = 10,
            Interval = 0.5f,
            ChargeDelay = 0.5f
        };
        _throwSkillData = new ThrowSkillData
        {
            Cooldown = 10f,
            Range = 10f,
            ChargeDelay = 0.5f,
            SyringeSpeed = 10f
        };
        _dashSkillData = new DashSkillData
        {
            Cooldown = 20f,
            Width = 5f,
            Speed = 20f,
            ChargeDelay = 0.8f
        };

    }

}
