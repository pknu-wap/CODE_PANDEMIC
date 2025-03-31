using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//namespace Inventory.Model
//{

//    public abstract class ItemData : ScriptableObject
//    {
//        public int MyPro { get; set; }
//        [field: SerializeField]
//        public bool isStackable { get; set; }

//        public int ID => GetInstanceID();
//        [field: SerializeField]
//        public int MaxStackSize { get; set; } = 99;

//        [field: SerializeField]
//        public string Name { get; set; }
//        [field: SerializeField]
//        [field: TextArea]
//        public string Description { get; set; }

//        [field: SerializeField]
//        public Sprite ItemImage { get; set; }

//        [field: SerializeField]
//        public List<ItemParameter> _parameterList = new List<ItemParameter>();
//    }
//    [Serializable]
//    public struct ItemParameter : IEquatable<ItemParameter>
//    {
//        public ItemParameterSO itemParameter;
//        public float value;

//        public bool Equals(ItemParameter other)
//        {
//            return other.itemParameter == itemParameter;
//        }
//    }
//}
