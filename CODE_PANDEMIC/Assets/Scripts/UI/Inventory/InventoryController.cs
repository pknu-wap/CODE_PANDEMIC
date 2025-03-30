using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
       
        [SerializeField] private UI_Inventory _inventoryUI;

       // private PlayerInput _inputActions;
        [SerializeField]
        private InventoryData _inventoryData;

        public UI_Inventory UIInventory 
        { 
            get { return _inventoryUI; } 
            private set { _inventoryUI = value; }
        }
        
        
        public List<InventoryItem> _initialItems = new List<InventoryItem>();
       
       //이후 init
        private void Start()
        {
            if (UIInventory == null)
            {
                Debug.LogError("InventoryUI가 할당되지 않았습니다.");
                return;
            }
            PrepareInventoryUI();
            PrepareInventoryData();
            // _inventoryData.Init();
        }
   
        private void PrepareInventoryData()
        {
            _inventoryData.Init();
            _inventoryData.OnInventoryChanged += UpdateInventoryUI;
            foreach (InventoryItem item in _initialItems)
            {
                if (item.IsEmpty) continue;
                _inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
          
            UIInventory.ResetAllItems();
            foreach(var item in inventoryState)
            {
                UIInventory.UpdateData(item.Key, item.Value._item.ItemImage, item.Value._quantity);
            }
        }
      

        private void PrepareInventoryUI()
        {
            UIInventory.InitializeInventoryUI(_inventoryData.Size);
            UIInventory.OnDescriptionRequested += HandleDescriptionRequest;
            UIInventory.OnSwapItems += HandleSwapItems;
            UIInventory.OnstartDragging += HandleDragging;
            UIInventory.OnItemActionRequested += HandleItemActionRequest;
           
        }

        private void HandleItemActionRequest(int index)
        {
       
            InventoryItem inventoryItem = _inventoryData.GetItemAt(index);
            if (inventoryItem.IsEmpty) return;
            IItemAction itemAction = inventoryItem._item as IItemAction;
            if (itemAction != null)
            {
                UIInventory.ShowItemAction(index);
                UIInventory.AddAction(itemAction.ActionName, () => PerformAction(index));
            }

            IDestroyableItem destroyableItem = inventoryItem._item as IDestroyableItem;

            if (destroyableItem != null)
            {
                UIInventory.AddAction("Drop", () => DropItem(index, inventoryItem._quantity));
            }
           
        }

        private void DropItem(int index, int quantity)
        {
            _inventoryData.RemoveItem(index, quantity);
            UIInventory.ResetSelection();
        }

        public void  PerformAction(int index)
        {
            InventoryItem inventoryItem = _inventoryData.GetItemAt(index);
            if (inventoryItem.IsEmpty) return;
            IDestroyableItem destroyableItem = inventoryItem._item as IDestroyableItem;
            if (destroyableItem != null)
            {
                _inventoryData.RemoveItem(index, 1);
            }
            IItemAction itemAction = inventoryItem._item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem._itemState);
                
                if (_inventoryData.GetItemAt(index).IsEmpty) UIInventory.ResetSelection();
            }
        }
        private void HandleDragging(int index)
        {
            InventoryItem inventoryItem = _inventoryData.GetItemAt(index);
            if (inventoryItem.IsEmpty) return;
            UIInventory.CreateDraggedItem(inventoryItem._item.ItemImage, inventoryItem._quantity);
        }

        private void HandleSwapItems(int index_1, int index_2)
        {
            _inventoryData.SwapItems(index_1, index_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = _inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                UIInventory.ResetSelection();
                return;
            } 
            string description = PrepareDescription(inventoryItem);
            ItemData item = inventoryItem._item;
            UIInventory.UpdateDescription(itemIndex, item.ItemImage, item.name,description);
        }
        private string PrepareDescription(InventoryItem item)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(item._item.Description);
            sb.AppendLine();
            for(int i=0; i<item._itemState.Count;i++)
            {
               sb.Append($"{item._itemState[i].itemParameter.ParameterName} " +
                    $": {item._itemState[i].value} / " +
                    $"{item._item._parameterList[i].value}");
                sb.AppendLine();
            }
            return sb.ToString();   
        }
        //private void OnEnable()
        //{
        //    _inputActions.PlayerActions.Item.performed -= ShowHide; // 중복 방지
        //    _inputActions.PlayerActions.Item.performed += ShowHide;
        //}

        //private void OnDisable()
        //{
        //    _inputActions.PlayerActions.Item.performed -= ShowHide;
        //}

        private void ShowHide(InputAction.CallbackContext context)
        {

            if (!UIInventory.gameObject.activeSelf)
            {
                UIInventory.Show();
                foreach (var item in _inventoryData.GetCurrentInventoryState())
                {
                    UIInventory.UpdateData(
                        item.Key,
                        item.Value._item.ItemImage,
                        item.Value._quantity
                        );
                }
            }
            else
            {
                UIInventory.Hide();
            }
        }
    }
}