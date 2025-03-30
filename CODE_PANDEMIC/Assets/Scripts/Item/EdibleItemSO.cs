using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO :ItemData,IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<ModifierData>_modifierData =new List<ModifierData>();
        //나중에 eventmanager로 빼자 
        public string ActionName => "Consume";

        public bool PerformAction(GameObject character , List<ItemParameter> itemState = null)
        {
            foreach(ModifierData data in _modifierData)
            {
                data._statModifier.AffectCharacter(character, data._value);
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
        public CharacterStatModifierSO _statModifier;
        public float _value;
    }
}