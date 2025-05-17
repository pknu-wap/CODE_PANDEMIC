using Unity.VisualScripting;
using UnityEngine;

public class AI_ThunderZombie : AI_Controller
{
    public float _thunderCoolDown = 10f;
    public float _thunderDelay = 1f;
    public float _attackCooldown = 5f;
    public float _attackDuration = 0.1f;

    [SerializeField] private GameObject attackColliderPrefab;
    [SerializeField] private Transform hitboxSpawnPoint;
    [SerializeField] private GameObject thunderPrefab;

    public LayerMask TargetLayer;
    private float _lastThunderTime = -Mathf.Infinity;

    public override float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    public Transform Player => _player.transform;
    private AI_ThunderSkill _thunderSkill;
    private AI_PatientAttack _patientAttack;

    protected override void Awake()
    {
        TargetLayer = LayerMask.GetMask("Player");
        base.Awake();
    }
    protected override void Start()
    {
        // if (_monsterData == null)
        // {
        //     _monsterData = new MonsterData();
        //     _monsterData.NameID = "ThunderZombie";
        //     _monsterData.Hp = 500;
        //     _monsterData.AttackDelay = 5.0f;
        //     _monsterData.DetectionRange = 7.5f;
        //     _monsterData.DetectionAngle = 180;
        //     _monsterData.MoveSpeed = 4f;
        //     _monsterData.AttackRange = 10f;
        //     _monsterData.AttackDamage = 30;
        // }
        base.Start();
        if (!Init())
        {
            enabled = false;
            return;
        }
        ChangeState(new AI_StateIdle(this));
        _thunderSkill = new AI_ThunderSkill(thunderPrefab, TargetLayer);
        _patientAttack = new AI_PatientAttack(attackColliderPrefab, hitboxSpawnPoint, _attackCooldown, _attackDuration);
    }
    public override void TryUseSkill(System.Action onSkillComplete)
    {
        if (_player == null)
        {
            onSkillComplete?.Invoke();
            return;
        }
        float now = Time.time;
        float distance = Vector2.Distance(transform.position, _player.position);

        if (now >= _lastThunderTime + _thunderCoolDown)
        {
            _lastThunderTime = now;
            _isUsingSkill = true;
            Debug.Log("ThunderZombie: Using Thunder Skill");
            _thunderSkill.StartSkill(this, onSkillComplete);
            _isUsingSkill = false;

        }
        else
        {
            Debug.Log("ThunderZombie: Not Attack");
            onSkillComplete?.Invoke();
        }
    }
    public override bool IsPlayerInSkillRange()
    {
        if (_player == null) return false;
        float distance = Vector2.Distance(transform.position, _player.position);
        return distance <= AttackRange * 0.7f;
    }
    private void AttackHit()
    {
        Debug.Log("ThunderZombie: Attack Hit");
        _patientAttack.StartSkill(this, null);
    }

}
