using Inventory.Model;
using Inventory.Model.Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        private UI_Inventory _inventoryUI;
        
        [SerializeField] 
        private InventoryData _inventoryData;

        private PlayerInput _inputAction;
        public List<InventoryItem> _initialItems = new List<InventoryItem>();

        public UI_Inventory UIInventory
        {
            get { return _inventoryUI; }
            private set { _inventoryUI = value; }
        }

      
        private void Start()
        {
            _inventoryData = Managers.Game.Inventory;
            if (_inventoryData == null)
            {
                Debug.LogError("InventoryData가 초기화되지 않았습니다.");
                return;
            }

            
            StartCoroutine(CoWaitLoad(() =>
            {
                Managers.UI.ShowInventoryUI((inventoryUI) =>
                {
                    _inventoryUI = inventoryUI;
                    PrepareInventoryUI();
                    PrepareInventoryData();
                });
            }));
        }
        private IEnumerator CoWaitLoad(Action callback)
        {
            // SceneUI가 null이 아니게 될 때까지 대기
            while (Managers.UI.SceneUI == null)
            {
                yield return null;
            }
            // 대기 후 콜백 실행
            callback?.Invoke();
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
                string address = item.Value._item.Sprite;
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

            UIInventory.ShowItemAction(index);

            if (inventoryItem._item is ItemData itemData)
            {
                if (itemData.Type == Define.ItemType.Equippable || itemData.Type == Define.ItemType.Edible)
                {
                    UIInventory.AddAction("Slot", () => SlotItem(index, inventoryItem._quantity));
                }
            }
            // Drop
            IDestroyableItem destroyableItem = inventoryItem._item as IDestroyableItem;
            if (destroyableItem != null)
            {
                UIInventory.AddAction("Drop", () => DropItem(index, inventoryItem._quantity));
            }

            // Slot
        }
        private void SlotItem(int index, int quantity)
        {
            InventoryItem inventoryItem = _inventoryData.GetItemAt(index);
            if (inventoryItem.IsEmpty) return;

            ItemData itemData = inventoryItem._item;
            Debug.Log("SlotItem");
            Managers.Game.QuickSlot.RegisterQuickSlot(itemData, quantity);
            _inventoryData.RemoveItem(index, quantity);
            UIInventory.ResetSelection();
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
            string address = inventoryItem._item.Sprite;
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
            string address = item.Sprite;
            Managers.Resource.LoadAsync<Sprite>(address, (loadedSprite) =>
            {
                if (loadedSprite != null)
                {
                    UIInventory.UpdateDescription(itemIndex, loadedSprite, item.Name, description);
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
            for (int i = 0; i < item._item.Parameters.Count; i++)
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

        public void ShowHide(InputAction.CallbackContext context)
        {
            if (!UIInventory.gameObject.activeSelf)
            {
                UIInventory.Show();
                foreach (var item in _inventoryData.GetCurrentInventoryState())
                {
                    string address = item.Value._item.Sprite;
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