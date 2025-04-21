using Inventory.UI;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Inventory.Model
{
    public class EdibleItem : ItemData, IDestroyableItem, IItemAction
    {
        public Define.ActionType ActionType => Define.ActionType.QuickSlot;

   
        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            var parametersToUse = itemState;

            foreach (ItemParameter param in parametersToUse)
            {
                switch (param.parameterName)
                {
                    case "Health":
                       PlayerStatus obj= Managers.Object.Player.GetComponent<PlayerStatus>();
                        obj.OnHealed(param.value);
                        break;

                        // ���� Stamina, Mana, Buff �� Ȯ�� ����
                }
            }

            return true;
        }
    }

    public interface  IDestroyableItem
    {
        //CanDestroy
    }
    public interface  IItemAction
    {
        Define.ActionType ActionType { get; }
        bool PerformAction(GameObject character, List<ItemParameter> itemState );
    }
    

}

