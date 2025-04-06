using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private WeaponBase equippedWeapon;
    [SerializeField] private GameObject macePrefab;
    [SerializeField] private GameObject gunPrefab;

    void Start()
    {
        if (macePrefab != null)
        {
            EquipWeapon(Instantiate(macePrefab, transform).GetComponent<WeaponBase>());
        }
    }

    void Update()
    {
        HandleWeaponSwap();
    }

    private void HandleWeaponSwap()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwapWeapon(macePrefab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwapWeapon(gunPrefab);
        }
    }

    private void SwapWeapon(GameObject weaponPrefab)
    {
        if (weaponPrefab == null)
        {
            return;
        }

        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }

        WeaponBase newWeapon = Instantiate(weaponPrefab, transform).GetComponent<WeaponBase>();

        if (newWeapon != null)
        {
            EquipWeapon(newWeapon);
        }
    }

    public void EquipWeapon(WeaponBase newWeapon)
    {
        equippedWeapon = newWeapon;
        equippedWeapon.transform.parent = transform;
        equippedWeapon.transform.localPosition = Vector3.zero;
    }
}
