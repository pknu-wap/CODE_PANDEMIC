using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private AnimatorOverrideController withArmOverride;
    [SerializeField] private AnimatorOverrideController noArmOverride;

    private Animator _animator;
    private EquipWeapon _equipWeapon;
    private bool _prevHasWeapon = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _equipWeapon = GetComponent<EquipWeapon>();
    }

    private void LateUpdate()
    {
        Transform socket = _equipWeapon.WeaponSocket;
        if (socket == null) return;

        bool hasWeapon = socket.childCount > 0;
        if (hasWeapon != _prevHasWeapon)
        {
            _animator.runtimeAnimatorController = hasWeapon ? noArmOverride : withArmOverride;
            _prevHasWeapon = hasWeapon;
        }
    }

    public void UpdateAnimationStates(bool isMoving, bool isRunning, bool isDashing)
    {
        _animator.SetBool("isWalking", isMoving && !isRunning);
        _animator.SetBool("isRunning", isMoving && isRunning);
        _animator.SetBool("isDashing", isDashing);
    }

    public void SetDeadAnimation()
    {
        _animator.SetBool("isDead", true);
    }
}