using Inventory.Model;
using Inventory.Model.Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{

    [SerializeField] private InventoryData _inventoryData;
    private void Start()
    {
        _inventoryData = Managers.Game.Inventory;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Item item = other.gameObject.GetComponent<Item>();
        
        if (item != null)
        {
            Managers.Game.AddItemCount();
            Managers.Event.InvokeEvent("ItemReward", item.InventoryItem);
            int reminder = _inventoryData.AddItem(item.InventoryItem, item.Quantity);
            if (reminder == 0) item.DestroyItem();
            else item.Quantity = reminder;
        }
    }
  
}
