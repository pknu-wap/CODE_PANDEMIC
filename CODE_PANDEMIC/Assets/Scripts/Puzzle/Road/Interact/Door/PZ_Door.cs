using UnityEngine;

public class PZ_Door : PZ_Interact_NonSpawn
{
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private Animator _animator;

    public override void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        base.Interact(player);

        _animator.SetBool("IsOpened", true);

        _boxCollider.isTrigger = true;
    }
}