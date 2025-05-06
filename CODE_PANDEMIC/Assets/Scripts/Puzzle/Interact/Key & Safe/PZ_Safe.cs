using UnityEngine;
using System.Collections;

public class PZ_Safe : PZ_Interact_Spawn
{
    [SerializeField] private Animator _animator;

    public bool _hasKey = false; // 열쇠로 열기
    private int _key;

    // 하이라이트 기능
    [SerializeField] private Material _lockMaterial;
    [SerializeField] private Material _unLockMaterial;

    private void Update()
    {
        _key = HasKey();

        if (_key == -1)
        {
            _hasKey = false;
        }
        else
        {
            _hasKey = true;
        }
    }

    public override void Interact(GameObject player)
    {
        if (_isInteracted || _key == -1)
        {
            Debug.Log("열쇠가 없음");
            return;
        }

        base.Interact(player);

        RemoveKeyAndReward(_key);

        _animator.SetBool("IsOpened", true);

        StartCoroutine(DestroyThisObject());
    }

    private int HasKey()
    {
        int key = Managers.Game.Inventory.HasItem(_interactData.KeyID);
        return key;
    }

    private void RemoveKeyAndReward(int key)
    {
        if (key != -1)
        {
            Managers.Game.Inventory.RemoveItem(key, 1);
            RewardItem();
        }
       
    }
    private IEnumerator DestroyThisObject()
    {
        yield return new WaitForSeconds(0.5f);

        float currentTime = 0f;

        while (currentTime < 1)
        {
            currentTime += Time.fixedDeltaTime;

            Color color = _spriteRenderer.color;
            color.a -= 0.02f;
            _spriteRenderer.color = color;

            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);

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