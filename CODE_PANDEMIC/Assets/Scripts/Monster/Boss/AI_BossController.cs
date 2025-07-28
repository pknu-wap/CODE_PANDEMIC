using System.Collections;
using UnityEngine;

public abstract class AI_BossController : AI_Controller
{
    [Header("Boss Cinematic")]
    [SerializeField] private CinematicCamera _camera;
    [SerializeField] protected ShockWave _wave;
    protected bool _isCinematicStarted = false;
    protected int MaxHealth => _monsterData.Hp;
    public bool IsBerserk { get; protected set; }
    

    protected override void Start()
    {
        base.Start();
    }
    public void StartBossFight()
    {
        StartCoroutine(BossStartSequence());
    }

    protected IEnumerator BossStartSequence()
    {
        if (_camera == null) yield break;
        _camera.gameObject.SetActive(true);
        _camera.OnCinematic();
        Managers.Event.InvokeEvent("OnCinematicStart");
        //TODO: BOSSSEQUENCE
        yield return CoroutineHelper.WaitForSeconds(2.0f);
        _wave.gameObject.SetActive(true);
        _wave.CallShockWave();
        yield return CoroutineHelper.WaitForSeconds(1.0f);
        _camera.OnEndCinematic(Define.CinematicType.BossSequence);
        Managers.Event.InvokeEvent("OnCinematicEnd");
    }


    public override void TakeDamage(int amount)
    {
        if (_isDead) return;
        if (!IsBerserk && Health <= MaxHealth * 0.5f)
        {
            EnterBerserkMode();
        }
        base.TakeDamage(amount);
        if (Health <= 0 && !_isDead)
        {
            Dielogic();
        }
        
    }
    protected override void Dielogic()
    {
        if (_isDead) return;
        _isDead = true;
        Skill?.StopSkill();
        _rb.velocity = Vector2.zero;
        StopMoving();
        Managers.Game.ClearBoss(_monsterData.TemplateID);
        ChangeState<AI_StateDie>();
    }

    protected virtual void EnterBerserkMode()
    {
        IsBerserk = true;
    }

    public override void DieAnimationEnd()
    {
        StartCoroutine(BossDeathSequence());
        base.DieAnimationEnd();
    }

    protected IEnumerator BossDeathSequence()
    {
        Managers.Event.InvokeEvent("OnBossClear");
        yield return CoroutineHelper.WaitForSeconds(1);
    }
}
