using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.U2D;
    using static Define;

    namespace Inventory.Model
    {
        public class InventoryData
        {
            [field: SerializeField]
            public List<InventoryItem> _inventoryItems;

            [field: SerializeField]
            public int Size { get; private set; } = 30;

            public event Action<Dictionary<int, InventoryItem>> OnInventoryChanged;

            public void Init()
            {
                _inventoryItems = new List<InventoryItem>();
                for (int i = 0; i < Size; i++)
                {
                    _inventoryItems.Add(InventoryItem.GetEmptyItem());
                }
            }
            public int AddItem(ItemData item, int quantity, List<ItemParameter> itemState = null)
            {
               
                ItemData newItem = ItemFactoryManager.CreateItem(item.Type, item);
                if (newItem == null)
                {
                    Debug.LogError("������ ���� ����");
                    return quantity;
                }

                if (newItem.Type == ItemType.Edible && Managers.Game.QuickSlot.HasItem(item))
                {
                    quantity = Managers.Game.QuickSlot.AddStackableItem( quantity);
                    InformAboutChange();
                    return quantity;
                }

                if (!newItem.IsStackable)
                {
                    while (quantity > 0 && !IsInventoryFull())
                    {
                        
                        quantity -= AddItemToFreeSlot(newItem, 1, itemState);
                    }
                }
                else // ���� ������ ���
                {
                    
                    quantity = AddStackableItem(newItem, quantity);
                }

                InformAboutChange();
                return quantity;
            }
        

            private int AddItemToFreeSlot(ItemData item, int quantity, List<ItemParameter> itemState = null)
            {
                InventoryItem newItem = new InventoryItem(item, quantity, new List<ItemParameter>(itemState == null ? item.Parameters : itemState));
                for (int i = 0; i < _inventoryItems.Count; i++)
                {
                    if (_inventoryItems[i].IsEmpty)
                    {
                        _inventoryItems[i] = newItem;
                        return quantity;
                    }
                }
                return 0;
            }

            private bool IsInventoryFull() => _inventoryItems.Where(item => item.IsEmpty).Any() == false;

            private int AddStackableItem(ItemData item, int quantity)
            {
                for (int i = 0; i < _inventoryItems.Count; i++)
                {
                    if (_inventoryItems[i].IsEmpty) continue;
                    if (_inventoryItems[i]._item.TemplateID == item.TemplateID)
                    {
                        int amountPossibleToTake = _inventoryItems[i]._item.MaxStackSize - _inventoryItems[i]._quantity;
                        if (quantity > amountPossibleToTake)
                        {
                            _inventoryItems[i] = _inventoryItems[i].ChangeQuantity(_inventoryItems[i]._item.MaxStackSize);
                            quantity -= amountPossibleToTake;
                        }
                        else
                        {
                            _inventoryItems[i] = _inventoryItems[i].ChangeQuantity(_inventoryItems[i]._quantity + quantity);
                            InformAboutChange();
                            return 0;
                        }
                    }
                }
                while (quantity > 0 && IsInventoryFull() == false)
                {
                    int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                    quantity -= newQuantity;
                    AddItemToFreeSlot(item, newQuantity);
                }
                return quantity;
            }

            public void AddItem(InventoryItem item)
            {
                AddItem(item._item, item._quantity);
            }

            public Dictionary<int, InventoryItem> GetCurrentInventoryState()
            {
                Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
                for (int i = 0; i < _inventoryItems.Count; i++)
                {
                    if (_inventoryItems[i].IsEmpty) continue;
                    returnValue[i] = _inventoryItems[i];
                }
                return returnValue;
            }

            public InventoryItem GetItemAt(int itemIndex)
            {
                return _inventoryItems[itemIndex];
            }

            public void SwapItems(int index_1, int index_2)
            {
                InventoryItem item1 = _inventoryItems[index_1];
                _inventoryItems[index_1] = _inventoryItems[index_2];
                _inventoryItems[index_2] = item1;
                InformAboutChange();
            }

            public void RemoveItem(int itemIndex, int amount)
            {
                if (_inventoryItems.Count > itemIndex)
                {
                    if (_inventoryItems[itemIndex].IsEmpty) return;
                    int reminder = _inventoryItems[itemIndex]._quantity - amount;
                    if (reminder <= 0) _inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                    else _inventoryItems[itemIndex] = _inventoryItems[itemIndex].ChangeQuantity(reminder);
                    InformAboutChange();
                }
            }
            public int HasItem(int id)
            { 
                var index = _inventoryItems
                    .FindIndex(item => !item.IsEmpty && item._item.TemplateID == id);

                if (index >= 0)
                {
                  
                    return index;
                }

                return -1;
            }
            
            private void InformAboutChange()
            {
                OnInventoryChanged?.Invoke(GetCurrentInventoryState());
            }
            #region Load(Inventory)
            public void LoadInventoryFromData(Dictionary<int, InventoryItem> loadedItems)
            {   
                _inventoryItems=new List<InventoryItem>();
                _inventoryItems.Clear();
                for (int i = 0; i < Size; i++)
                {
                    _inventoryItems.Add(InventoryItem.GetEmptyItem());
                }

                foreach (var item in loadedItems)
                {
                    _inventoryItems[item.Key] = item.Value;
                }
                InformAboutChange();
            }
            #endregion

        }
    }


    [Serializable]
    //heap �� �ƴ� stack�� �Ҵ��� ���� 
    public struct InventoryItem
    {
        public int _quantity;
        public ItemData _item;
        public List<ItemParameter> _itemState;
        public InventoryItem(ItemData item, int quantity, List<ItemParameter> itemState = null) : this()
        {
            _quantity = quantity;
            _item = item;
            _itemState = itemState ?? new List<ItemParameter>();
        }

        public bool IsEmpty => _item == null;
        public InventoryItem ChangeQuantity(int quantity)
        {
           
            return new InventoryItem(
                _item, 
                quantity,
                _itemState
                );
        
        }
        public static InventoryItem GetEmptyItem() => new InventoryItem
        {
            _item = null,
            _quantity = 0,
            _itemState = new List<ItemParameter>()
        };


    }

}