using Inventory.Model;
using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private int itemID; // ItemData�� ID�� ������ ����

    public ItemData InventoryItem { get; private set; }
    public int Quantity { get; set; } = 1;

    [SerializeField]
    private float duration = 0.3f;

    private void Start()
    {
        // DataManager���� ItemData�� �ε��Ͽ� �Ҵ�
        if (Managers.Data != null && Managers.Data.Items.TryGetValue(itemID, out ItemData itemData))
        {
            InventoryItem = itemData;
            // ��������Ʈ �ε� �� �Ҵ� (�ʿ��� ���)
            
        }
        else
        {
            Debug.LogError($"ItemData with ID {itemID} not found.");
            Destroy(gameObject); // ItemData�� ã�� �� ������ ������ ����
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