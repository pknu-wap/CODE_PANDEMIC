using System.Collections;
using UnityEngine;

public class AI_BossDash : AI_DashSkill
{
    private AI_HospitalBoss _boss;
    private Coroutine _dashCoroutine;
    private Rigidbody2D _rigid;

    public void SetController(AI_BossController controller)
    {
        base.SetController(controller);
        _boss = controller as AI_HospitalBoss;
    }

    public override bool IsReady(AI_Controller controller)
    {
        if (_boss == null || !_boss.IsBerserk)
            return false;

        return base.IsReady(controller);
    }

    public override void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {
        if (!IsReady(controller))
        {
            onSkillComplete?.Invoke();
            return;
        }

        _boss._isUsingSkill = true;
        _boss._aiPath.canMove = false;

        _dashCoroutine = _boss.StartCoroutine(DashRoutine(onSkillComplete));
    }

    protected override IEnumerator DashRoutine(System.Action onSkillComplete)
    {
        Vector2 start = _boss.transform.position;
        Vector2 target = _boss._player.position;

        Vector2 direction = (target - start).normalized;
        float distance = Vector2.Distance(start, target) + 2f;
        float duration = distance / DashSpeed;

        float elapsed = 0f;

        Rigidbody2D rb = _boss.GetComponent<Rigidbody2D>();
        bool _hasHitPlayer = false;
        Transform playerTransform = _boss._player;
        float dashCheckRadius = DashWidth;

        yield return new WaitForSeconds(DashDuration);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (rb != null)
                rb.velocity = direction * DashSpeed;

            if (!_hasHitPlayer)
            {
                float currentDistance = Vector2.Distance(_boss.transform.position, playerTransform.position);
                if (currentDistance <= dashCheckRadius)
                {
                    _hasHitPlayer = true;

                    if (playerTransform.TryGetComponent<PlayerStatus>(out var playerStatus))
                    {
                        int damage = Mathf.RoundToInt(_boss.AiDamage * 1.5f);
                        playerStatus.OnDamaged(_boss.gameObject, damage);
                    }
                }
            }

            yield return null;
        }

        if (rb != null)
            rb.velocity = Vector2.zero;

        _boss._aiPath.canMove = true;
        _boss._isUsingSkill = false;

        onSkillComplete?.Invoke();
    }

    public override void StopSkill()
    {
        if (_dashCoroutine != null)
        {
            _boss.StopCoroutine(_dashCoroutine);
            _dashCoroutine = null;
            _rigid = _boss.GetComponent<Rigidbody2D>();

            if (_rigid != null)
                _rigid.velocity = Vector2.zero;

            _boss._aiPath.canMove = true;
            _boss._isUsingSkill = false;
        }
    }
}
