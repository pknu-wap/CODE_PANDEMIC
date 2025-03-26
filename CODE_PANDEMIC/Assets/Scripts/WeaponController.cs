using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform attackPoint;

    void Update()
    {
      
        if (Input.GetMouseButtonDown(0))
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        
        Vector3 direction = mousePosition - attackPoint.position;

        
        GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Launch(direction);
    }
}
