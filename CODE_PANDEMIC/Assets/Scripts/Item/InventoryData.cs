using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model
{
    public class InventoryData : MonoBehaviour
    {
        [field: SerializeField]
        public List<InventoryItem> _inventoryItems;
        

        [field: SerializeField]
        public int Size { get; private set; } = 10;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryChanged; 
        public void Init()
        {
            _inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                _inventoryItems.Add(InventoryItem.GetEmptyItem());

            }
        }
        public int AddItem(ItemData item, int quantity,List<ItemParameter> itemState=null)
        {
            if (!item.isStackable) // 스택 불가능한 경우
            {
                while (quantity > 0 && !IsInventoryFull())
                {
                  quantity-=  AddItemToFreeSlot(item, 1,itemState);
                  
                }
            }
            else // 스택 가능한 경우
            {
                quantity = AddStackableItem(item, quantity);
            }

            InformAboutChange(); 
            return quantity; 
        }


        private int AddItemToFreeSlot(ItemData item, int quantity, List<ItemParameter> itemState = null)
        {
            InventoryItem  newItem = new InventoryItem
                (item,
                quantity,
                new List<ItemParameter>(itemState==null? item._parameterList:itemState)
                );
            for(int i  =0; i<_inventoryItems.Count;i++)
            {
                if (_inventoryItems[i].IsEmpty)
                {
                    _inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }
        private bool IsInventoryFull() =>
            _inventoryItems.Where(item => item.IsEmpty).Any() == false;
      

        private int AddStackableItem(ItemData item, int quantity)
        {
            for (int i = 0; i <_inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty) continue;
                if (_inventoryItems[i]._item.ID == item.ID)
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
                AddItemToFreeSlot(item,newQuantity);
            }
            return quantity;
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item._item, item._quantity);
        }
        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue
                = new Dictionary<int, InventoryItem>();
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
           InventoryItem item1= _inventoryItems[index_1];
            _inventoryItems[index_1] = _inventoryItems[index_2];
            _inventoryItems[index_2] = item1;
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryChanged?.Invoke(GetCurrentInventoryState());
        }

       public void RemoveItem(int itemIndex, int amount)
        {
            if(_inventoryItems.Count>itemIndex)
            {
                if (_inventoryItems[itemIndex].IsEmpty) return;
                int reminder = _inventoryItems[itemIndex]._quantity - amount;
                if (reminder <= 0) _inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                else _inventoryItems[itemIndex] = _inventoryItems[itemIndex].ChangeQuantity(reminder);
                InformAboutChange();
            }
           
        }
    }
    
    [Serializable]
    //heap 이 아닌 stack에 할당을 위함 
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