using System.Collections;
using UnityEngine;

public class AI_ThunderSkill : ISkillBehavior
{
    protected AI_Controller _controller;
    private Coroutine _skillCoroutine;
    private float _cooldown = 10f;
    private float _lastSkillTime = -Mathf.Infinity;

    private float _delayBeforeStrike = 0.5f;
    private float _intervalBetweenStrikes = 1.0f;
    private int _strikeCount = 3;
    private float _strikeRadius = 1f;

    private GameObject _thunderPrefab;
    private LayerMask _targetLayer;

    public AI_ThunderSkill(GameObject thunderPrefab, LayerMask targetLayer)
    {
        _thunderPrefab = thunderPrefab;
        _targetLayer = targetLayer;
    }

    public void SetController(AI_Controller controller)
    {
        _controller = controller;
    }

    public bool IsReady(AI_Controller controller)
    {
        return Time.time >= _lastSkillTime + _cooldown;
    }

    public void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {
        if (!IsReady(controller))
        {
            onSkillComplete?.Invoke();
            return;
        }

        _controller = controller;
        _controller._animator.SetBool("Skill", true);
        _lastSkillTime = Time.time;
        _controller._isUsingSkill = true;
        _controller._aiPath.canMove = false;
        _skillCoroutine = _controller.StartCoroutine(SkillRoutine(onSkillComplete));

    }


    private IEnumerator SkillRoutine(System.Action onSkillComplete)
    {
        for (int i = 0; i < _strikeCount; i++)
        {
            Vector3 strikePosition = _controller._player.position;
            GameObject thunderArea = GameObject.Instantiate(_thunderPrefab, strikePosition, Quaternion.identity);
            ThunderStrike strikeComponent = thunderArea.GetComponent<ThunderStrike>();

            if (strikeComponent != null)
            {
                int damage = Mathf.RoundToInt(_controller.Damage * 1.2f);
                strikeComponent.Initialize(damage, _delayBeforeStrike, _strikeRadius, _targetLayer);
            }

            yield return new WaitForSeconds(_intervalBetweenStrikes);
        }

        _controller._isUsingSkill = false;
        _controller._aiPath.canMove = true;
        onSkillComplete?.Invoke();
        _controller._animator.SetBool("Skill", false);
    }

    public void StopSkill()
    {
        if (_skillCoroutine != null && _controller != null)
        {
            _controller.StopCoroutine(_skillCoroutine);
        }
    }
    public virtual void SetSettings(object settings, LayerMask targetLayer, AI_Controller controller)
{
    _controller = controller;
    _targetLayer = targetLayer;
    // _settings = settings as ThunderSkillData;
}
}
