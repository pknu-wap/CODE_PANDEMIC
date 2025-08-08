using System.Collections;
using UnityEngine;

public class AI_ThunderSkill : AI_SkillBase
{
    private Coroutine _skillCoroutine;
    private float _lastSkillTime = -Mathf.Infinity;
    protected ThunderSkillData _settings;

    [SerializeField] private GameObject _thunderPrefab;

    public override void SetSettings(object settings, LayerMask targetLayer, AI_Controller controller)
    {
        base.SetSettings(settings, targetLayer, controller);
        _settings = settings as ThunderSkillData;
    }

    public override bool IsReady(AI_Controller controller)
    {
        return Time.time >= _lastSkillTime + _settings.Cooldown;
    }

    public override void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {
        if (!IsReady(controller))
        {
            onSkillComplete?.Invoke();
            return;
        }

        _controller = controller;
        _controller._animator.SetBool("Skill", true);
        _lastSkillTime = Time.time;
        _movement._isUsingSkill = true;
        _controller._movement.StopMoving();
        _skillCoroutine = _controller.StartCoroutine(SkillRoutine(onSkillComplete));
    }

    private IEnumerator SkillRoutine(System.Action onSkillComplete)
    {
        for (int i = 0; i < _settings.StrikeCount; i++)
        {
            Vector3 strikePosition = _controller._detection.Player.position;
            GameObject thunderArea = GameObject.Instantiate(_thunderPrefab, strikePosition, Quaternion.identity);
            ThunderStrike strikeComponent = thunderArea.GetComponent<ThunderStrike>();

            if (strikeComponent != null)
            {
                int damage = Mathf.RoundToInt(_controller.Damage * 1.2f);
                strikeComponent.Initialize(damage, _settings.DelayBeforeStrike, _settings.StrikeRadius, _targetLayer);
            }

            yield return new WaitForSeconds(_settings.IntervalBetweenStrikes);
        }

        _movement._isUsingSkill = false;
        _controller._movement.ChasePlayer();
        onSkillComplete?.Invoke();
        _controller._animator.SetBool("Skill", false);
    }

    public override void StopSkill()
    {
        if (_skillCoroutine != null && _controller != null)
        {
            _controller.StopCoroutine(_skillCoroutine);
        }
    }
}
