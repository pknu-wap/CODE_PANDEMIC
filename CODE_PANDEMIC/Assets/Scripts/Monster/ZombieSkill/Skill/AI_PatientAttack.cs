using System.Collections;
using UnityEngine;

public class AI_PatientAttack : ISkillBehavior
{
    private GameObject _hitboxPrefab;
    private Transform _spawnPoint;
    private float _cooldown;
    private float _duration;
    private float _lastUsedTime;
    private Coroutine _currentCoroutine;

    public void SetController(AI_Controller ai)
    {
    }

    public AI_PatientAttack(GameObject hitboxPrefab, Transform spawnPoint, float cooldown, float duration)
    {
        _hitboxPrefab = hitboxPrefab;
        _spawnPoint = spawnPoint;
        _cooldown = cooldown;
        _duration = duration;
    }

    public bool IsReady(AI_Controller ai)
    {
        return Time.time >= _lastUsedTime + _cooldown;
    }

    public void StartSkill(AI_Controller ai, System.Action onComplete)
    {
        _lastUsedTime = Time.time;
        ai._isUsingSkill = true;
        ai._aiPath.canMove = false;

        _currentCoroutine = ai.StartCoroutine(SkillRoutine(ai, onComplete));
    }

    private IEnumerator SkillRoutine(AI_Controller ai, System.Action onComplete)
    {
        Vector3 spawnPos = _spawnPoint.position;
        if (ai.transform.localScale.x < 0)
        {
            float offset = _spawnPoint.localPosition.x;
            spawnPos = ai.transform.position + new Vector3(-offset, _spawnPoint.localPosition.y, 0f);
        }

        GameObject hitbox = GameObject.Instantiate(_hitboxPrefab, spawnPos, Quaternion.identity);
        LayerMask targetLayer = LayerMask.GetMask("Player");
        hitbox.GetComponent<AttackCollider>().Initialize((int)ai.Damage, _duration, targetLayer);

        yield return new WaitForSeconds(_duration);

        ai._isUsingSkill = false;
        ai._aiPath.canMove = true;
        onComplete?.Invoke();
    }

    public void StopSkill()
    {
        // Future use: stop coroutine if necessary
    }
}
