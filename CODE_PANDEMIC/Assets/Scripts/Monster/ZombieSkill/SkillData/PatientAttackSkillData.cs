using UnityEngine;

public class PatientAttackSkillData
{
    public float Cooldown { get; set; }
    public float Duration { get; set; }
    public GameObject HitboxPrefab { get; set; }
    public Transform SpawnPoint { get; set; }
}