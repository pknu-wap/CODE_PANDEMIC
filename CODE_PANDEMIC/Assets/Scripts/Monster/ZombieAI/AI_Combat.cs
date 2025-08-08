using UnityEngine;

public class AI_Combat : MonoBehaviour
{
    private bool _isAttack;

    public ISkillBehavior Skill { get; set; }
    public bool IsAttacking => _isAttack;

    public void StartAttack(System.Action onAttackStarted)
    {
        if (_isAttack) return;

        _isAttack = true;
        onAttackStarted?.Invoke();
    }

    public void StopAttack(System.Action onAttackStopped)
    {
        if (!_isAttack) return;

        _isAttack = false;
        onAttackStopped?.Invoke();
    }

    public void StopSkill()
    {
        Skill?.StopSkill();
    }
}