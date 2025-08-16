using UnityEngine;

public class GrenadeSkillData
{
    public float Cooldown { get; set; }
    public float ThrowDelay { get; set; }
    public float Damage { get; set; }
    public float ExplosionRadius { get; set; }
    public float ExplosionDelay { get; set; }
    public GameObject GrenadePrefab { get; set; }
    public GameObject ExplosionEffect { get; set; }
}
