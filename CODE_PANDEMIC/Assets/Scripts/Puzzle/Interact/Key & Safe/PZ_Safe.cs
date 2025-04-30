using UnityEngine;

public class PZ_Safe : PZ_Interact_Spawn
{
    [SerializeField] private Animator _animator;

    public bool _hasKey = true; // 열쇠로 열기

    // 하이라이트 기능
    [SerializeField] private Material _lockMaterial;
    [SerializeField] private Material _unLockMaterial;

    public override void Interact(GameObject player)
    {
        int key = HasKey();
        if (_isInteracted ||key==-1)
        {
            Debug.Log("열쇠가 없음");
            return;
        }

        base.Interact(player);

        RewardItem(key); 
      
        Destroy(gameObject);
        _animator.SetBool("IsOpened", true);

        // 금고 해제 및 무기 획득
    }
 
    public override void OnHighLight()
    {
        if (_hasKey)
        {
            _spriteRenderer.material = _unLockMaterial;
        }
        else
        {
            _spriteRenderer.material = _lockMaterial;
        }
    }
}