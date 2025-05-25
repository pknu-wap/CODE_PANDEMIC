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

    public bool HasWeapon()
    {
        return _weapon!=null;
    }

    public Transform WeaponSocket => _socket;
    public void Attack(PlayerController owner)
    {
        _weapon?.Attack(owner);
    }

    public void StartAttack(PlayerController owner)
    {
        _weapon?.StartAttack(owner);
    }

    public void StopAttack()
    {
        _weapon?.StopAttack();
    }

    public void Equip(WeaponBase weapon)
    {
        _weapon = weapon;
    }

   

    public void SetWeapon(WeaponItem weaponItem, List<ItemParameter> itemState)
    {
        if (CheckReloading()==false) return;
        Managers.Data.Weapons.TryGetValue(weaponItem.TemplateID, out WeaponData data);
        if (data == null)
        {
            Debug.Log("None Data");
            return;
        }
        switch (data.Type) //for various of sockets  if just one socket remove switch 
        {
            case Define.WeaponType.ShortWeapon:
                if (!CheckDifferentWeapon(data, _weapon))
                {
                    UnEquipWeapon();
                }
                else
                {
                    SwapWeapon(data,_socket);
                    Managers.Event.InvokeEvent("ShortWeaponEquipped");
                }
                break;
            case Define.WeaponType.PistolWeapon:
                if (!CheckDifferentWeapon(data, _weapon))
                {
                    UnEquipWeapon();
                }
                else
                {
                    SwapWeapon(data, _socket);
                    Managers.Event.InvokeEvent("GunWeaponEquipped", data.BulletCount);
                }
                    break;
            case Define.WeaponType.RangeWeapon:
                if (!CheckDifferentWeapon(data, _weapon))
                {
                    UnEquipWeapon();
                }
                else
                {
                    SwapWeapon(data, _socket);
                    Managers.Event.InvokeEvent("GunWeaponEquipped", data.BulletCount);
                }
                break;
            default:
                break;
        }

    }
    bool CheckDifferentWeapon(WeaponData item, WeaponBase currentWeapon)
    {
        if (currentWeapon == null) return true; //장착이 아무것도 안되어있음 

        if (item.TemplateID == currentWeapon.ID) return false; //현재 장착되
        else return true;
    }
    bool CheckReloading()
    {
        if (_weapon == null) return true;
        else
        {
            if (_weapon.IsReloading) return false;
        }
        return true;
    }
    private void DestroyPrevWeapon()
    {
        if (_weapon == null) return;
        Destroy(_weapon.gameObject);
        _weapon = null;
    }
    private void UnEquipWeapon()
    {
        DestroyPrevWeapon();
    }

    public void SwapWeapon(WeaponData data,Transform socket)
    {
        DestroyPrevWeapon();
        Managers.Resource.Instantiate(data.WeaponPrefab, socket, (obj) =>
        {
            _weapon = obj.GetComponent<WeaponBase>();
            _weapon.SetInfo(data);

        });

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