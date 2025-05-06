using UnityEngine;

public class Mes : CloseRangeWeaponBase
{
    [SerializeField] private GameObject firePoint;
    private bool isThrown = false;
    private Vector3 direction;
    private float speed;
    private int damage;

    private void Update()
    {
        if (isThrown)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public override void Attack(PlayerController owner)
    {
        if (!CanFire() || isThrown) return;
        SetNextFireTime();

        isThrown = true;
        direction = firePoint.transform.right.normalized;
        speed = _weaponData.BulletSpeed;
        damage = _weaponData.Damage;

        Debug.Log("Mes가 던져졌습니다.");
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        {
            AI_Base enemy = other.GetComponent<AI_Base>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        
        }

        Debug.Log("Mes가 적과 충돌했습니다.");
        Destroy(gameObject);
    }
}
