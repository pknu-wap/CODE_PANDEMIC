using UnityEngine;
public class GunSkillData
{
    public float Cooldown { get; set; }
    public float FireRate { get; set; }
    public int StrikeCount { get; set; }
    public float Range { get; set; }
    public float Damage { get; set; }
    public GameObject BulletPrefab { get; set; }
    public float BulletSpeed { get; set; }
}