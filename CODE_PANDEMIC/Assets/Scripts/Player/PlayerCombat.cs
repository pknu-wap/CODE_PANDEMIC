using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private WeaponBase equippedWeapon;
    [SerializeField] private GameObject macePrefab;
    [SerializeField] private GameObject gunPrefab;

    private float fireTimer;

    void Start()
    {
        if (macePrefab != null)
        {
            EquipWeapon(Instantiate(macePrefab, transform).GetComponent<WeaponBase>());
        }
    }

    void Update()
    {
        HandleAttack();
        HandleWeaponSwap();
    }

    private void HandleAttack()
    {
        if (equippedWeapon != null && Input.GetMouseButtonDown(0) && fireTimer <= 0f)
        {
            equippedWeapon.Attack();
            fireTimer = equippedWeapon.fireRate;
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }
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
            Debug.LogWarning("WeaponPrefab is null!");
            return;
        }

        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }

        WeaponBase newWeapon = Instantiate(weaponPrefab, transform).GetComponent<WeaponBase>();

        if (newWeapon == null)
        {
            Debug.LogError("WeaponBase component is missing from the prefab!");
            return;
        }

        EquipWeapon(newWeapon);
        Debug.Log("Weapon Swapped to: " + newWeapon.weaponName);
    }

    public void EquipWeapon(WeaponBase newWeapon)
    {
        equippedWeapon = newWeapon;
        equippedWeapon.transform.parent = transform;
        equippedWeapon.transform.localPosition = Vector3.zero;
    }
}

