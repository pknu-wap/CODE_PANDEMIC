using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private EquipWeapon _equipWeapon;

    private PlayerMovement _playerMovement; 
    private PlayerStatus _playerStatus; 
    private PlayerInteraction _playerInteraction;

    public PlayerState _currentState = PlayerState.Idle; 

    public Vector2 _forwardVector;

    [SerializeField] public Transform _weaponHolder;
    
    #region Base

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerStatus = GetComponent<PlayerStatus>();
        _playerInteraction = GetComponent<PlayerInteraction>();
        _equipWeapon = GetComponent<EquipWeapon>();  
    }

    private void OnEnable()
    {
        Managers.Event.Subscribe("OnPlayerDead", OnPlayerDead);
    }

    private void OnDisable()
    {
     
        Managers.Event.Unsubscribe("OnPlayerDead", OnPlayerDead);
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

    private void OnPlayerDead(object obj)
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