using UnityEngine;
using Inventory.Model;
using System.Collections.Generic;

public class EquipWeapon : MonoBehaviour
{
    [SerializeField]
    private WeaponBase _weapon;
    [SerializeField]
    private GameObject _socket;

    private QuickSlot _quickSlot;
    private PlayerInput _weaponInput;

    private void Awake()
    {
        _weaponInput = new PlayerInput();
    }

    private void Start()
    {
        _quickSlot = Managers.Game.QuickSlot;
    }

    private void OnEnable()
    {
        _weaponInput.QuickSlot.Equip1.performed += Equip1;
        _weaponInput.QuickSlot.Equip2.performed += Equip2;
        _weaponInput.QuickSlot.Equip3.performed += Equip3;
        _weaponInput.QuickSlot.Equip4.performed += Equip4;
        _weaponInput.Enable();
    }

    private void OnDisable()
    {
        _weaponInput.QuickSlot.Equip1.performed -= Equip1;
        _weaponInput.QuickSlot.Equip2.performed -= Equip2;
        _weaponInput.QuickSlot.Equip3.performed -= Equip3;
        _weaponInput.QuickSlot.Equip4.performed -= Equip4;
        _weaponInput.Disable();
    }

    public void Equip(WeaponBase newWeapon)
    {
        if (_weapon != null)
        {
            Destroy(_weapon.gameObject);
        }

        _weapon = newWeapon;
        _weapon.transform.SetParent(_socket.transform);
        _weapon.transform.localPosition = Vector3.zero;
        _weapon.transform.localRotation = Quaternion.identity;

        Rigidbody2D rb = _weapon.GetComponent<Rigidbody2D>();
        if (rb != null) Destroy(rb);
        Collider2D collider = _weapon.GetComponent<Collider2D>();
        if (collider != null) Destroy(collider);
    }


    public void Attack()
    {
        _weapon?.Attack();
    }

    private bool EquipQuickSlot(int v)
    {
        if (!_quickSlot.CheckSlot(v))
            return false;

        _quickSlot.UseQuickSlot(v, gameObject);
        return true;
    }

    private void Equip1(UnityEngine.InputSystem.InputAction.CallbackContext ctx) => EquipQuickSlot(1);
    private void Equip2(UnityEngine.InputSystem.InputAction.CallbackContext ctx) => EquipQuickSlot(2);
    private void Equip3(UnityEngine.InputSystem.InputAction.CallbackContext ctx) => EquipQuickSlot(3);
    private void Equip4(UnityEngine.InputSystem.InputAction.CallbackContext ctx) => EquipQuickSlot(4);
}
