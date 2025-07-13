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
        StartCoroutine(BossStartSequence());
        base.Start();
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
        base.TakeDamage(amount);

        if (!IsBerserk && Health <= MaxHealth * 0.5f)
        {
            EnterBerserkMode();
        }

        if (Health <= 0f && _currentState is not AI_StateDie)
        {
            _isDead = true;
            Skill?.StopSkill();
            _rb.velocity = Vector2.zero;
            StopMoving();
            Managers.Game.ClearBoss(_monsterData.TemplateID);
            StartCoroutine(BossDeathSequence());
            ChangeState(new AI_StateDie(this));
        }
    }

    protected virtual void EnterBerserkMode()
    {
        IsBerserk = true;
    }

    protected IEnumerator BossDeathSequence()
    {
        Managers.Event.InvokeEvent("OnBossClear");
        yield return CoroutineHelper.WaitForSeconds(2);
    }
}
