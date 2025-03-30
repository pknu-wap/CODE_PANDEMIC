using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UI_Inventory :UI_Base //이후에 UIBASE
    {
        public enum Images
        {
            Frame
        }
        public enum GameObjects
        {
            InventoryContent,
            Description,
            MouseFollower,
            ItemActionPanel
        }
        //[SerializeField]
        // UIInventoryItem _item;
        [SerializeField]
        RectTransform _contentPanel;

        List<UI_InventoryItem> _listOfUiItems = new List<UI_InventoryItem>();

        [SerializeField]
        UIInventoryDescription _description;
        [SerializeField]
        MouseFollower _mouseFollower;
        [SerializeField]
        ItemActionPanel _actionPanel;

        public event Action<int> OnDescriptionRequested,
            OnItemActionRequested, OnstartDragging;

        public event Action<int, int> OnSwapItems;

        //-1은 item 없음 
        int _currentlyDraggedItemIndex = -1;
        
        public override bool Init()
        {
            if (base.Init() == false) return false;
            BindImage(typeof(Images));
            BindObject(typeof(GameObjects));

            _contentPanel = Utils.FindChild(GetObject((int)GameObjects.InventoryContent), "BG", true).GetComponent<RectTransform>();
            _description = GetObject((int)GameObjects.Description)?.GetComponent<UIInventoryDescription>();
            _mouseFollower=GetObject((int)GameObjects.MouseFollower)?.GetComponent<MouseFollower>();    
            _actionPanel=GetObject((int)GameObjects.ItemActionPanel)?.GetComponent<ItemActionPanel>();

            _mouseFollower.StopFollowing();
            _description.ResetDescription();
            Hide();
            
            return true;
        }
        public void InitializeInventoryUI(int inventorySize)
        {
            int count = 0;
            for (int i = 0; i < inventorySize; i++)
            {
                Managers.Resource.Instantiate("ItemSlot", callback: (obj) =>
                {
                    UI_InventoryItem uiItem = obj.GetComponent<UI_InventoryItem>();
                    uiItem.transform.SetParent(_contentPanel);

                    if (uiItem == null)
                    {
                        Debug.LogError($"[InstantiateUIItem] {obj}에서 UIInventoryItem을 찾을 수 없습니다.");
                        return;
                    }

                    _listOfUiItems.Add(uiItem);

                    // 이벤트 핸들러 연결
                    uiItem.OnItemClicked += HandleItemSelection;
                    uiItem.OnItemBeginDrag += HandleBeginDrag;
                    uiItem.OnItemDroppedOn += HandleSwap;
                    uiItem.OnItemEndDrag += HandleEndDrag;
                    uiItem.OnRightMouseButtonClick += HandleShowItemActions;
                    uiItem.Initialize();
                    count++; 
                    if(count==inventorySize)
                    {
                        Hide();
                    }
                });
            }

        }
        
        public void UpdateData(int itemIndex,
            Sprite itemImage, int itemQuantity)
        {
            if (_listOfUiItems.Count > itemIndex)
                _listOfUiItems[itemIndex].SetData(itemImage, itemQuantity);
        }
        private void HandleShowItemActions(UI_InventoryItem item)
        {
            int index = _listOfUiItems.IndexOf(item);
            if (index == -1)
            {
                ResetDraggedItem();
                return;
            }
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(UI_InventoryItem item)
        {
            _mouseFollower.StopFollowing();
        }

        private void HandleSwap(UI_InventoryItem item)
        {
            
            int index = _listOfUiItems.IndexOf(item);
            if (index == -1)
            {
                ResetDraggedItem();
                return;
            }
            OnSwapItems?.Invoke(_currentlyDraggedItemIndex, index);
        }

        private void ResetDraggedItem()
        {
            _mouseFollower.StopFollowing();
            _currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(UI_InventoryItem item)
        {
            int index = _listOfUiItems.IndexOf(item);
            if (index == -1) return;
            _currentlyDraggedItemIndex = index;
            HandleItemSelection(item);
            OnstartDragging?.Invoke(index);

        }
        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            _mouseFollower.StartFollowing(sprite, quantity);
        }

        private void HandleItemSelection(UI_InventoryItem item)
        {
            int index = _listOfUiItems.IndexOf(item);
            if (index == -1) return;
            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            gameObject.SetActive(true);

            ResetSelection();

        }

        public void ResetSelection()
        {
            _description.ResetDescription();
            DeselectAllItems();
        }

        private void DeselectAllItems()
        {
            foreach (var item in _listOfUiItems)
            {
                item.Deselect();
            }
            _actionPanel.Toggle(false);
        }
        public void AddAction(string actionName ,Action performAction)
        {
            _actionPanel.AddButton(actionName, performAction);
        }
        public  void ShowItemAction(int itemIndex)
        {
            _actionPanel.Toggle(true);
            _actionPanel.transform.position = _listOfUiItems[itemIndex].transform.position;
        }
        public void Hide()
        {
            _actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        public void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            _description.SetDescription(itemImage, name, description);
            DeselectAllItems();
            _listOfUiItems[itemIndex].Select();
        }

        public void ResetAllItems()
        {
           foreach(var item  in _listOfUiItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}