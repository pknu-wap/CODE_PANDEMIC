using System.Collections;
using UnityEngine;

public class AI_ThrowSkill : ISkillBehavior
{
    private Coroutine _skillCoroutine;
    private float _lastSkillTime = -Mathf.Infinity;
    private AI_NurseZombie _currentNurse;
    private Transform _player;
    public AI_ThrowVisualizer _throwVisualizer;

    public void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {
        if (!IsReady(controller))
        {
            var callback = onSkillComplete;
            if (callback != null)
                callback();
            return;
        }
        _currentNurse = controller as AI_NurseZombie;
        _throwVisualizer = _currentNurse._throwVisualizer;
        _lastSkillTime = Time.time;
        _skillCoroutine = controller.StartCoroutine(ThrowRoutine(controller as AI_NurseZombie, onSkillComplete));
    }

    public void StopSkill()
    {
        // 필요 시 StopCoroutine 추가 가능
    }

    private IEnumerator ThrowRoutine(AI_NurseZombie nurse, System.Action onSkillComplete)
{
    var aiPath = nurse._aiPath;
    aiPath.canMove = false;

    Vector2 attackDirection = (nurse.Player != null)
        ? ((Vector2)nurse.Player.position - (Vector2)nurse.transform.position).normalized
        : nurse.transform.right;
    if (_throwVisualizer != null)
    {
        _throwVisualizer.transform.position = nurse.transform.position;
        _throwVisualizer.Show(nurse.Player.position, nurse.SkillChargeDelay);
    }
    yield return new WaitForSeconds(nurse.SkillChargeDelay);

    ThrowSyringe(nurse, attackDirection);

    if (_throwVisualizer != null)
        _throwVisualizer.Hide();

    aiPath.canMove = true;


        var callback = onSkillComplete;
        if (callback != null)
            callback();
    }

    private void ThrowSyringe(AI_NurseZombie nurse, Vector2 direction)
    {
        Vector2 spawnPos = nurse.transform.position;
        GameObject projectile = Object.Instantiate(nurse._syringePrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = direction * nurse.SyringeSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    public bool IsReady(AI_Controller controller)
    {
        if (_currentNurse == null)
            _currentNurse = controller as AI_NurseZombie;

        if (_currentNurse != null)
            return Time.time >= _lastSkillTime + _currentNurse.SkillCooldown;

        return false;
    }
}
