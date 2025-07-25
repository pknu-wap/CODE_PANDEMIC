using UnityEngine;
using Inventory.Model;
using System.Collections.Generic;
using Unity.VisualScripting;
using Cinemachine;
using System.Collections;
using static Define;

public class EquipWeapon : MonoBehaviour
{

    [SerializeField]
    private WeaponBase _weapon;
    [SerializeField]
    private Transform _socket;

    private QuickSlot _quickSlot;
    private PlayerInput _weaponInput;

    private float _equipDelay = 0.35f;
    Coroutine _equipDelayRoutine;

    private void Awake()
    {
        _weaponInput = new PlayerInput();
    }

    private void Start()
    {
        Managers.Event.InvokeEvent("EquipDisable");
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
        Managers.Event.InvokeEvent("CancelReload");
        StopDelay();
        _weaponInput.QuickSlot.Equip1.performed -= Equip1;
        _weaponInput.QuickSlot.Equip2.performed -= Equip2;
        _weaponInput.QuickSlot.Equip3.performed -= Equip3;
        _weaponInput.QuickSlot.Equip4.performed -= Equip4;
        _weaponInput.Disable();
    }

    public bool HasWeapon()
    {
        return _weapon != null;
    }
    public void Reload()
    {
        if (_weapon.WeaponInfo.Type == Define.WeaponType.ShortWeapon || _weapon == null) return;

        _weapon.Reload();
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
        if (CheckReloading() == false) return;
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
                    SwapWeapon(data, _socket);
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
    public void UnEquipWeapon()
    {
        Managers.Event.InvokeEvent("EquipDisable");
        StopDelay();
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
        if (!_quickSlot.CheckSlot(v))
            return false;
        if (_equipDelayRoutine != null) return false;

        _quickSlot.UseQuickSlot(v, gameObject);

        _equipDelayRoutine = StartCoroutine(EquipDelay());
        return true;
    }
    IEnumerator EquipDelay()
    {
        yield return CoroutineHelper.WaitForSeconds(_equipDelay);
        _equipDelayRoutine = null;
    }
    private void StopDelay()
    {
        if (_equipDelayRoutine == null) return;
        StopCoroutine(_equipDelayRoutine );
        _equipDelayRoutine = null;  
    }
    private void Equip1(UnityEngine.InputSystem.InputAction.CallbackContext ctx) => EquipQuickSlot(1);
    private void Equip2(UnityEngine.InputSystem.InputAction.CallbackContext ctx) => EquipQuickSlot(2);
    private void Equip3(UnityEngine.InputSystem.InputAction.CallbackContext ctx) => EquipQuickSlot(3);
    private void Equip4(UnityEngine.InputSystem.InputAction.CallbackContext ctx) => EquipQuickSlot(4);

    #region 테스트용
    public void SetWeaponWithPrefab(WeaponItem weaponItem, List<ItemParameter> itemState, GameObject prefab)
    {
        if (!CheckReloading()) return;

        if (!Managers.Data.Weapons.TryGetValue(weaponItem.TemplateID, out WeaponData data))
        {
            Debug.LogWarning("무기 데이터가 존재하지 않습니다.");
            return;
        }

        data.WeaponPrefab = prefab.name; 

        switch (data.Type)
        {
            case WeaponType.ShortWeapon:
                if (!CheckDifferentWeapon(data, _weapon))
                {
                    UnEquipWeapon();
                }
                else
                {
                    SwapWeaponWithPrefab(data, _socket, prefab);
                    Managers.Event.InvokeEvent("ShortWeaponEquipped");
                }
                break;

            case WeaponType.PistolWeapon:
            case WeaponType.RangeWeapon:
                if (!CheckDifferentWeapon(data, _weapon))
                {
                    UnEquipWeapon();
                }
                else
                {
                    SwapWeaponWithPrefab(data, _socket, prefab);
                    Managers.Event.InvokeEvent("GunWeaponEquipped", data.BulletCount);
                }
                break;
        }
    }
    private void SwapWeaponWithPrefab(WeaponData data, Transform socket, GameObject prefab)
    {
        DestroyPrevWeapon();

        GameObject weaponInstance = Instantiate(prefab, socket);
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localRotation = Quaternion.identity;

        _weapon = weaponInstance.GetComponent<WeaponBase>();
        if (_weapon != null)
        {
            _weapon.SetInfo(data);
        }
        else
        {
            Debug.LogError("프리팹에 WeaponBase 컴포넌트가 없습니다.");
        }
    }
    #endregion
}