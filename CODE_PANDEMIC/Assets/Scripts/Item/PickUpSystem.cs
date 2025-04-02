using Inventory.Model;
using Inventory.Model.Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{

    [SerializeField] private InventoryData _inventoryData;
   
    private void OnTriggerEnter(Collider other)
    {
        Item item = other.gameObject.GetComponent<Item>();
        Debug.Log("abc");
        if (item != null)
        {
            int reminder = _inventoryData.AddItem(item.InventoryItem, item.Quantity);
            if (reminder == 0) item.DestroyItem();
            else item.Quantity = reminder;
        }
    }
}
