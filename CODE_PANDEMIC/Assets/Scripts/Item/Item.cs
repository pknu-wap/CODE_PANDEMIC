using Inventory.Model;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private int itemID; // ItemData의 ID를 저장할 변수
    public ItemData InventoryItem { get; private set; }
    public int Quantity { get; set; } = 1;
    SpriteRenderer _spriteRenderer;
    [SerializeField]
    private float duration = 0.3f;

    private void Start()
    {
        _spriteRenderer= gameObject.GetOrAddComponent<SpriteRenderer>();    
        // DataManager에서 ItemData를 로드하여 할당
        if (Managers.Data != null && Managers.Data.Items.TryGetValue(itemID, out ItemData itemData))
        {
            InventoryItem = itemData;
            Managers.Resource.LoadAsync<Sprite>(InventoryItem.Sprite, callback: (obj) =>
            {
                _spriteRenderer.sprite = obj;
            });
          
        }
        else
        {
            Debug.LogError($"ItemData with ID {itemID} not found.");
            Destroy(gameObject); // ItemData를 찾을 수 없으면 아이템 삭제
        }
            
    }

    public void DestroyItem()
    {
        GetComponent<Collider>().enabled = false;
        StartCoroutine(AnimateItempPickUp());
    }

    private IEnumerator AnimateItempPickUp()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        transform.localScale = endScale;
        Destroy(gameObject);
    }
}