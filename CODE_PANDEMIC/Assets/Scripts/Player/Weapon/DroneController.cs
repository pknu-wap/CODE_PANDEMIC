using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class DroneController : MonoBehaviour
{
    [Header("Drone Settings")]
    public float moveSpeed = 5f;
    public float detectionRange = 10f;
    public float fireRate = 1f;
    public float bulletSpeed = 15f;

    [Header("Dependencies")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Follow Settings")]
    public float followDistance = 2f;
    public float followLerpSpeed = 5f;

    private Transform playerTransform;
    private Transform target;
    private float nextFireTime;

    private bool isActive = false;
    private Coroutine droneCycleCoroutine;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        nextFireTime = Time.time;
    }

    void Update()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                playerTransform = playerObj.transform;
        }

        // Q 키를 누를 때 드론 사이클 시작/종료
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (droneCycleCoroutine == null)
            {
                droneCycleCoroutine = StartCoroutine(DroneCycle());
            }
            else
            {
                StopCoroutine(droneCycleCoroutine);
                droneCycleCoroutine = null;
                isActive = false;
                Debug.Log("Drone 사이클 종료");
            }
        }

        if (!isActive || playerTransform == null) return;

        FollowPlayer();
        FindTarget();

        if (target != null)
        {
            AimAtTarget();
            TryFire();
        }
    }

    void FollowPlayer()
    {
        Vector3 desiredPosition = playerTransform.position + (transform.position - playerTransform.position).normalized * followDistance;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followLerpSpeed);
    }

    IEnumerator DroneCycle()
    {
        isActive = true;
        Debug.Log("Drone 활성화됨 (10초간)");
        yield return new WaitForSeconds(10f);

        isActive = false;
        Debug.Log("Drone 비활성화됨 (15초간)");
        yield return new WaitForSeconds(15f);

        droneCycleCoroutine = null;
    }

    void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") && hit.gameObject.activeInHierarchy) // 활성화된 적만 타겟으로
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

    void AimAtTarget()
    {
        if (target == null) return;
        Vector2 dir = target.position - firePoint.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    void TryFire()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Fire();
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * bulletSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        if (playerTransform != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(playerTransform.position, followDistance);
        }
    }
}