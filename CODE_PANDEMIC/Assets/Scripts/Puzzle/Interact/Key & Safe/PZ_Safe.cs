using UnityEngine;

public class PZ_Safe : PZ_Interact_Base
{
    public bool _hasKey = false; // 열쇠로 열기

    // 하이라이트 기능
    [SerializeField] private Material _lockMaterial;
    [SerializeField] private Material _unLockMaterial;

    public override void Interact(GameObject player)
    {
        if (_isInteracted || !_hasKey)
        {
            Debug.Log("열쇠가 없음");
            return;
        }

        base.Interact(player);

        // 금고 해제 및 무기 획득

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