using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemData InventoryItem { get; private set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    //[SerializeField]
    // AudioSource _audioSource;

    [SerializeField]
    private float duration = 0.3f;

    private void Start()
    {
        //   GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
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
