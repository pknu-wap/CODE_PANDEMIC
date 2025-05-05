using System;
using UnityEngine;

public class AI_PatientAttack : ISkillBehavior
{
    private GameObject _attackColliderPrefab;
    private float _cooldown = 1.5f;
    private float _lastSkillTime = -Mathf.Infinity;

    public AI_PatientAttack(GameObject attackColliderPrefab)
    {
        _attackColliderPrefab = attackColliderPrefab;
    }

    public bool IsReady(AI_Controller controller)
    {
        return Time.time >= _lastSkillTime + _cooldown;
    }

    public void StartSkill(AI_Controller controller, Action onComplete)
    {
        if (!IsReady(controller)) return;

        _lastSkillTime = Time.time;

        Vector2 offset = controller.transform.localScale.x > 0f ? Vector2.right : Vector2.left;
        Vector2 spawnPos = (Vector2)controller.transform.position + offset * 0.5f;
        // GameObject colliderObj = UnityEngine.Object.Instantiate(_attackColliderPrefab, spawnPos, Quaternion.identity);

        // colliderObj.transform.right = offset;

        onComplete?.Invoke();
    }

    public void StopSkill()
    {
    }

    public float GetCooldown()
    {
        return _cooldown;
    }
}
