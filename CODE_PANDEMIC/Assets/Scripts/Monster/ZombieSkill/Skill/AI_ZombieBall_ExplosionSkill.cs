using UnityEngine;

public class AI_ZombieBall_ExplosionSkill : AI_SkillBase
{
    public GameObject explosionEffectPrefab;
    public float explosionRadius = 2.5f;
    public float explosionDamageMultiplier = 1.1f;
    public float summonRadius = 1.5f;
    public GameObject zombiePrefab; // 소환할 좀비 프리팹

    public override bool IsReady(AI_Controller controller)
    {
        return true;
    }

    public override void StartSkill(AI_Controller controller, System.Action onSkillComplete)
    {
        _controller = controller;
        Explode();
        onSkillComplete?.Invoke();
    }

    public override void StopSkill()
    {
    }

    private void Explode()
    {
        if (_controller._isDead) return;

        if (explosionEffectPrefab)
        {
            Object.Instantiate(explosionEffectPrefab, _controller.transform.position, Quaternion.identity);
        }

        Collider2D[] targets = Physics2D.OverlapCircleAll(_controller.transform.position, explosionRadius, LayerMask.GetMask("Player"));
        int damage = Mathf.RoundToInt(_controller.AiDamage * explosionDamageMultiplier);

        foreach (var target in targets)
        {
            if (target.TryGetComponent<PlayerController>(out var playerStatus))
            {
                playerStatus.TakeDamage(_controller.gameObject, damage);
            }
        }

        TrySummon();
        _controller.GetComponent<AI_Health>().TakeDamage(_controller.GetComponent<AI_Health>().CurrentHp); // 자폭
    }

    private void TrySummon()
    {
        Transform player = _detection.Player;
        if (player == null)
            return;

        int summonCount = Random.Range(3, 6);
        if (Managers.Data.Monsters.TryGetValue(5, out MonsterData data))
        {
            for (int i = 0; i < summonCount; i++)
            {
                Managers.Resource.Instantiate(data.Prefab, null, (obj) =>
                {
                    Vector2 spawnPos = (Vector2)_controller.transform.position + Random.insideUnitCircle * summonRadius;
                    obj.transform.position = spawnPos;
                    obj.transform.SetParent(_controller.transform.parent, worldPositionStays: true);
                    obj.GetComponent<AI_Base>()?.SetInfo(data);
                    AI_Controller summonZombie = obj.GetComponent<AI_PatientZombie>();
                    if (summonZombie != null)
                    {
                        summonZombie.ForceDetectTarget(player);
                    }
                });
            }
        }
    }
}