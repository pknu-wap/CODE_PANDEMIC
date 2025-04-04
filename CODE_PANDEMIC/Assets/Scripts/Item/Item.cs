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
        _spriteRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();

        // 데이터가 초기화되었는지 먼저 확인
        if (Managers.Data == null)
        {
            Debug.LogError("Data manager" );
            return;
        }
        if ( Managers.Data.Items == null)
        {
            Debug.LogError(" item dictionary is null.");
            return;
        }
        // ItemData 로드 시도
        if (Managers.Data.Items.TryGetValue(itemID, out ItemData itemData))
        {
            Debug.Log($"ItemData found: {itemData.Name} (ID: {itemID})");
            InventoryItem = itemData;
            Managers.Resource.LoadAsync<Sprite>(InventoryItem.Sprite, callback: (obj) =>
            {
                _spriteRenderer.sprite = obj;
            });
        }
        else
        {
            Debug.LogError($"ItemData with ID {itemID} not found. Available IDs: {string.Join(", ", Managers.Data.Items.Keys)}");
            Destroy(gameObject);
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