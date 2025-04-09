using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Inventory.Model
{
    public class EdibleItem : ItemData,IDestroyableItem, IItemAction
    {
        public List<ItemParameter> parameters = new(); // JSON에서 직접 로드
        public string ActionName => "Consume";

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            var parametersToUse = itemState ?? parameters;

            foreach (ItemParameter param in parametersToUse)
            {
                switch (param.parameterName)
                {
                    case "Health":
                       PlayerStatus obj= Managers.Object.Player.GetComponent<PlayerStatus>();
                        obj.OnHealed(param.value);
                        break;

                        // 추후 Stamina, Mana, Buff 등 확장 가능
                }
            }

            return true;
        }
    }

    public interface  IDestroyableItem
    {

    }
    public interface  IItemAction
    {
        public string ActionName { get; }
        //public AudioClip actionSFX{get;}
        bool PerformAction(GameObject character, List<ItemParameter> itemState );
        
    }
    [Serializable]
    public class  ModifierData
    {
        public CharacterStatModifier _statModifier;
        public float _value;
    }
}