using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private PlayerController _playerController;

    //private GameObject currentWeapon;
    //public void EquipWeapon(GameObject newWeaponPrefab)
    //{
    //    if (currentWeapon != null)
    //    {
    //        Destroy(currentWeapon);
    //    }

    //    currentWeapon = Instantiate(newWeaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);
    //}

    public void EquipWeapon(WeaponBase newWeapon)
    {
        if (_playerController._equippedWeapon != null)
        {
            Destroy(_playerController._equippedWeapon.gameObject);
        }

        _playerController._equippedWeapon = newWeapon;
        _playerController._equippedWeapon.transform.SetParent(_playerController._weaponHolder);
        _playerController._equippedWeapon.transform.localPosition = Vector3.zero;
        _playerController._equippedWeapon.transform.localRotation = Quaternion.identity;
    }
}