using UnityEngine;

public class AI_DoctorZombie : AI_Controller
{
    public float SweepRange = 2f;
    public float SweepAngle = 90f;
    public int SweepCount = 5;
    public float SweepInterval = 0.5f;
    public float SkillCooldown = 15f;
    public float SkillChargeDelay = 2f;
    public LayerMask TargetLayer;
    public float AiDamage => _monsterData.AttackDamage;
    public string AIName => _monsterData.NameID;
    public Transform Player => _player;
    private ISkillBehavior _skill;
    public ISkillBehavior Skill { get; private set; }
    
    protected override void Start()
    {
        if (!Init())
        {
            enabled = false;
            return;
        }
        if (SweepVisualizer != null)
        {
            SweepVisualizer.Hide();

        }
        _skill = new AI_SweepSkill();
    }
}