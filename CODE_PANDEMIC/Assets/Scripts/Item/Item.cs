using Inventory.Model;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private int itemID;

    public ItemData InventoryItem { get; private set; }
    public int Quantity { get; set; } = 1;

    SpriteRenderer _spriteRenderer;
    [SerializeField] private float floatAmplitude = 0.1f; // 위아래 움직임 범위
    [SerializeField] private float floatFrequency = 2f;   // 움직임 속도
    private Vector3 _startPos;
    private void Start()
    {
        _spriteRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();   
        _startPos = transform.position;

        if (Managers.Data.Items.TryGetValue(itemID, out ItemData itemData))
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
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //sin 함수활용 -1~1
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = _startPos + new Vector3(0, yOffset, 0);

    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItempPickUp());
    }

    private IEnumerator AnimateItempPickUp()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        float duration = 0.3f;

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
