using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class DroneController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRange = 10f;
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private Transform target;
    private float nextFireTime;
    private bool isActive = false;
    private bool isRunningCycle = false;

    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame && !isRunningCycle)
        {
            StartCoroutine(DroneCycle());
        }

        if (!isActive) return;

        FindTarget();

        if (target != null)
        {
            MoveToTarget();
            AimAtTarget();
            TryFire();
        }
    }

    IEnumerator DroneCycle()
    {
        isRunningCycle = true;

        while (true)
        {
            // 드론 활성화
            isActive = true;
            Debug.Log("Drone 활성화됨");
            yield return new WaitForSeconds(10f);

            // 드론 비활성화
            isActive = false;
            Debug.Log("Drone 비활성화됨");
            yield return new WaitForSeconds(15f);
        }
    }

    void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closest = hit.transform;
                }
            }
        }

        target = closest;
    }

    void MoveToTarget()
    {
        Vector2 dir = (target.position - transform.position).normalized;
        transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
    }

    void AimAtTarget()
    {
        Vector2 dir = target.position - firePoint.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    void TryFire()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Fire();
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = firePoint.right * 10f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
