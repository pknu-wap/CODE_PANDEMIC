using UnityEngine;

public class PZ_Safe : PZ_Interact_Spawn
{
    public bool _hasKey = false; // 열쇠로 열기

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