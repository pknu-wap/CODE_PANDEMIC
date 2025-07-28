using System.Collections;
using TMPro;
using UnityEngine;

public class AI_PatientAttack : AI_SkillBase
{
    private GameObject _hitboxPrefab;
    private Transform _spawnPoint;
    private float _lastUsedTime;
    private Coroutine _skillCoroutine;
    protected PatientAttackSkillData _settings;

    public override void SetSettings(object settings, LayerMask targetLayer, AI_Controller controller)
    {
        base.SetSettings(settings, targetLayer, controller);
        _settings = settings as PatientAttackSkillData;
        _hitboxPrefab = _settings.HitboxPrefab;
        _spawnPoint = _settings.SpawnPoint;
        if (_hitboxPrefab != null)
            _hitboxPrefab.SetActive(false);
    }

    public override bool IsReady(AI_Controller controller)
    {
        return Time.time >= _lastUsedTime + _settings.Cooldown;
    }

    public override void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {
        if (!IsReady(controller))
        {
            onSkillComplete?.Invoke();
            return;
        }
        _controller = controller;
        _controller._animator.SetBool("Attack", true);
        _lastUsedTime = Time.time;
        _movement._isUsingSkill = true;
        _controller._movement.StopMoving();

        _skillCoroutine = _controller.StartCoroutine(SkillRoutine(onSkillComplete));
    }

    private IEnumerator SkillRoutine(System.Action onSkillComplete)
    {
        Vector3 spawnPos = _spawnPoint.position;
        if (_controller.transform.localScale.x < 0)
        {
            float offset = _spawnPoint.localPosition.x;
            spawnPos = _controller.transform.position + new Vector3(-offset, _spawnPoint.localPosition.y, 0f);
        }
        _hitboxPrefab.transform.position = spawnPos;
        _hitboxPrefab.SetActive(true);
        _hitboxPrefab.GetComponent<AttackCollider>().Initialize((int)_controller.Damage, _settings.Duration, LayerMask.GetMask("Player"));

        yield return new WaitForSeconds(_settings.Duration);
        _hitboxPrefab.SetActive(false);
        _movement._isUsingSkill = false;
        _movement.ChasePlayer();
        onSkillComplete?.Invoke();
    }

    public override void StopSkill()
    {
        if (_skillCoroutine != null && _controller != null)
        {
            _controller.StopCoroutine(_skillCoroutine);
        }
    }
}
