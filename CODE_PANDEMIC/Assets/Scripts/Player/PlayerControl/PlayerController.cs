using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 컴포넌트 관리
    private PlayerMovement _playerMovement; // 플레이어 무브먼트
    private PlayerStatus _playerStatus; // 플레이어 상태
    private PlayerInteraction _playerInteraction; // 플레이어 상호 작용
    private PlayerWeaponController _playerWeaponController; // 무기 컨트롤러

    public PlayerState _currentState = PlayerState.Idle; // 플레이어 상태

    public Vector2 _forwardVector; // 현재 플레이어가 바라보는 방향

    [SerializeField] public Transform _weaponHolder;
    public WeaponBase _equippedWeapon; // 장착 무기

    #region Base

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();

        _playerStatus = GetComponent<PlayerStatus>();

        _playerInteraction = GetComponent<PlayerInteraction>();

        _playerWeaponController = GetComponent<PlayerWeaponController>();
    }

    private void OnDisable()
    {
        _playerMovement.enabled = false;
        _playerStatus.enabled = false;
        _playerInteraction.enabled = false;
        _playerWeaponController.enabled = false;
    }

    #endregion

    #region Weapon

    public void EquipWeapon(WeaponBase newWeapon)
    {
        _playerWeaponController.EquipWeapon(newWeapon);
    }

    #endregion

    #region Status

    // 데미지 받는 함수
    public void TakeDamage(GameObject attacker, float damageValue)
    {
        _playerStatus.OnDamaged(attacker, damageValue);
    }

    // 힐 받는 함수
    public void TakeHeal(float healValue)
    {
        _playerStatus.OnHealed(healValue);
    }

    #endregion

    #region Die

    public void Die()
    {
        if (_currentState == PlayerState.Dead) return;

        _currentState = PlayerState.Dead;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        enabled = false;
    }

    public bool IsDead()
    {
        return _currentState == PlayerState.Dead;
    }

    #endregion
}