using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UI_Inventory _inventoryUI;
        [SerializeField] private InventoryData _inventoryData;
        public List<InventoryItem> _initialItems = new List<InventoryItem>();

        public UI_Inventory UIInventory
        {
            get { return _inventoryUI; }
            private set { _inventoryUI = value; }
        }

        private void Start()
        {
            _inventoryData = Managers.Game.Inventory;
            if (UIInventory == null)
            {
                Debug.LogError("InventoryUI가 할당되지 않았습니다.");
                return;
            }
            PrepareInventoryUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
           // _inventoryData.Init();
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
            foreach (var item in inventoryState)
            {
                // Addressable을 사용하여 이미지 로드
                string address = item.Value._item.ItemImageAddress;
                Managers.Resource.LoadAsync<Sprite>(address, (loadedSprite) =>
                {
                    if (loadedSprite != null)
                    {
                        UIInventory.UpdateData(item.Key, loadedSprite, item.Value._quantity);
                    }
                    else
                    {
                        Debug.LogError($"Failed to load sprite from address: {address}");
                    }
                });
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

        public void PerformAction(int index)
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
            string address = inventoryItem._item.ItemImageAddress;
            Managers.Resource.LoadAsync<Sprite>(address, (loadedSprite) =>
            {
                if (loadedSprite != null)
                {
                    UIInventory.CreateDraggedItem(loadedSprite, inventoryItem._quantity);
                }
                else
                {
                    Debug.LogError($"Failed to load sprite from address: {address}");
                }
            });
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
            string address = item.ItemImageAddress;
            Managers.Resource.LoadAsync<Sprite>(address, (loadedSprite) =>
            {
                if (loadedSprite != null)
                {
                    UIInventory.UpdateDescription(itemIndex, loadedSprite, item.name, description);
                }
                else
                {
                    Debug.LogError($"Failed to load sprite from address: {address}");
                }
            });
        }

        private string PrepareDescription(InventoryItem item)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(item._item.Description);
            sb.AppendLine();
            for (int i = 0; i < item._item.parameters.Count; i++)
            {
                string parameterInfo = item._item.GetParameterInfo(i);
                if (!string.IsNullOrEmpty(parameterInfo))
                {
                    sb.Append(parameterInfo);
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        private void ShowHide(InputAction.CallbackContext context)
        {
            if (!UIInventory.gameObject.activeSelf)
            {
                UIInventory.Show();
                foreach (var item in _inventoryData.GetCurrentInventoryState())
                {
                    string address = item.Value._item.ItemImageAddress;
                    Managers.Resource.LoadAsync<Sprite>(address, (loadedSprite) =>
                    {
                        if (loadedSprite != null)
                        {
                            UIInventory.UpdateData(item.Key, loadedSprite, item.Value._quantity);
                        }
                        else
                        {
                            Debug.LogError($"Failed to load sprite from address: {address}");
                        }
                    });
                }
            }
            else
            {
                UIInventory.Hide();
            }
        }
    }
}