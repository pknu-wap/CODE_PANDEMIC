using Inventory.Model;
using static Define;
using System.Collections.Generic;
using System;

public interface IItemFactory
{
    ItemData CreateItem(int templateID, string name, string description, bool isStackable, int maxStackSize, string sprite, WeaponType weapon, List<ItemParameter> parameters);
}

public class EdibleItemFactory : IItemFactory
{
    public ItemData CreateItem(int templateID, string name, string description, bool isStackable, int maxStackSize, string sprite, WeaponType weapon, List<ItemParameter> parameters)
    {
        return new ItemData
        {
            TemplateID = templateID,
            Name = name,
            Description = description,
            IsStackable = isStackable,
            MaxStackSize = maxStackSize,
            Sprite = sprite,
            Type = ItemType.Edible,
            Weapon = WeaponType.None,
            Parameters= parameters
        };
    }
}

public class EquippableItemFactory : IItemFactory
{
    public ItemData CreateItem(int templateID, string name, string description, bool isStackable, int maxStackSize, string sprite, WeaponType weapon, List<ItemParameter> parameters)
    {
        return new ItemData
        {
            TemplateID = templateID,
            Name = name,
            Description = description,
            IsStackable = isStackable,
            MaxStackSize = maxStackSize,
            Sprite = sprite,
            Type = ItemType.Equippable,
            Weapon = weapon,
            Parameters = parameters
        };
    }
}

public class ItemFactoryManager
{
    private static readonly IItemFactory EdibleFactory = new EdibleItemFactory();
    private static readonly IItemFactory EquippableFactory = new EquippableItemFactory();

    public static ItemData CreateItem(ItemType type, int templateID, string name, string description, bool isStackable, int maxStackSize, string sprite, WeaponType weapon, List<ItemParameter> parameters)
    {
        return type switch
        {
            ItemType.Edible => EdibleFactory.CreateItem(templateID, name, description, isStackable, maxStackSize, sprite, weapon, parameters),
            ItemType.Equippable => EquippableFactory.CreateItem(templateID, name, description, isStackable, maxStackSize, sprite, weapon, parameters),
            _ => throw new ArgumentException($"Unknown item type: {type}")
        };
    }
}
