using UnityEngine;
using Inventory.Model;
using System.Collections.Generic;
using Unity.VisualScripting;
using Cinemachine;

public class EquipWeapon : MonoBehaviour
{
    [SerializeField]
    private WeaponBase _weapon;

    private QuickSlot _quickSlot;
    private PlayerInput _weaponInput;

    [SerializeField] private Transform _socket;

    public bool HasWeapon()
    {
        return _socket.childCount >=1;
    }

    public Transform WeaponSocket => _socket;

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

    public void SetWeapon(WeaponItem weaponItem, List<ItemParameter> itemState)
    {

        Managers.Data.Weapons.TryGetValue(weaponItem.TemplateID, out WeaponData data);
        if (data == null)
        {
            Debug.Log("None Data");
            return;
        }
        switch (data.Type)
        {
            case Define.WeaponType.ShortWeapon:
                break;
            case Define.WeaponType.PistolWeapon:
                if (!CheckSameWeapon(data,_weapon)) return;

                DestroyPrevWeapon();
                Managers.Resource.Instantiate(data.WeaponPrefab, _socket.transform, (obj) =>
                {
                    _weapon = obj.GetComponent<WeaponBase>();
                    _weapon.SetInfo(data);
                   
                });
                break;
            case Define.WeaponType.RangeWeapon:
                break;
            default:
                break;
        }

    }
    bool CheckSameWeapon(WeaponData item , WeaponBase currentWeapon)
    {
        if (currentWeapon == null) return true; //장착이 아무것도 안되어있음 

        if (item.TemplateID == currentWeapon.ID) return false; //현재 장착되
        else return true;
    }

    private void DestroyPrevWeapon()
    {
        if (_weapon == null) return;
        Destroy(_weapon.gameObject);
        _weapon = null;
    }
    public void SwapWeapon(WeaponItem weaponItem, List<ItemParameter> itemState=null)
    {


    }
    public void Attack(PlayerController owner)
    {
        _weapon?.Attack(owner);
    }

    private bool EquipQuickSlot(int v)
    {
        Debug.Log(v);
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