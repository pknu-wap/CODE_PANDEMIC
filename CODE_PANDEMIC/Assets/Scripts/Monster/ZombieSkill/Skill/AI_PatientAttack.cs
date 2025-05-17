using System.Collections;
using UnityEngine;

public class AI_PatientAttack : ISkillBehavior
{
    protected AI_Controller _controller;
    private GameObject _hitboxPrefab;
    private Transform _spawnPoint;
    private float _cooldown;
    private float _duration;
    private float _lastUsedTime;
    private Coroutine _skillCoroutine;

    public void SetController(AI_Controller controller)
    {
        _controller = controller;
    }

    public AI_PatientAttack(GameObject hitboxPrefab, Transform spawnPoint, float cooldown, float duration)
    {
        _hitboxPrefab = hitboxPrefab;
        _spawnPoint = spawnPoint;
        _cooldown = cooldown;
        _duration = duration;
        if (_hitboxPrefab != null)
            _hitboxPrefab.SetActive(false); 
    }

    public bool IsReady(AI_Controller controller)
    {
        return Time.time >= _lastUsedTime + _cooldown;
    }

    public void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {
        if (!IsReady(controller))
        {
            onSkillComplete?.Invoke();
            return;
        }
        _controller = controller;
        _lastUsedTime = Time.time;
        _controller._isUsingSkill = true;
        _controller._aiPath.canMove = false;

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
        _hitboxPrefab.GetComponent<AttackCollider>().Initialize((int)_controller.Damage, _duration, LayerMask.GetMask("Player"));

        yield return new WaitForSeconds(_duration);
        _hitboxPrefab.SetActive(false);
        _controller._isUsingSkill = false;
        _controller._aiPath.canMove = true;
        onSkillComplete?.Invoke();
    }

    public void StopSkill()
    {
        if (_skillCoroutine != null && _controller != null)
        {
            _controller.StopCoroutine(_skillCoroutine);
        }
    }
}
