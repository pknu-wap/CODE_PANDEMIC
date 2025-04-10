using Inventory.Model;
using static Define;
using System;
using System.Collections.Generic;

public interface IItemFactory
{
    ItemData CreateItem(ItemData data);
}

public class EdibleItemFactory : IItemFactory
{
    public ItemData CreateItem(ItemData data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        return new EdibleItem
        {
            TemplateID = data.TemplateID,
            Name = data.Name,
            Description = data.Description,
            IsStackable = data.IsStackable,
            MaxStackSize = data.MaxStackSize,
            Sprite = data.Sprite,
            Type = ItemType.Edible,
            Weapon = WeaponType.None,
            Parameters = data.Parameters
        };
    }
}

public class EquippableItemFactory : IItemFactory
{
    public ItemData CreateItem(ItemData data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        return new EquippableItem
        {
            TemplateID = data.TemplateID,
            Name = data.Name,
            Description = data.Description,
            IsStackable = data.IsStackable,
            MaxStackSize = data.MaxStackSize,
            Sprite = data.Sprite,
            Type = ItemType.Equippable,
            Weapon = data.Weapon,
            Parameters = data.Parameters
        };
    }
}
public class ItemFactoryManager
{
    private static readonly Dictionary<ItemType, IItemFactory> FactoryMap = new Dictionary<ItemType, IItemFactory>
    {
        { ItemType.Edible, new EdibleItemFactory() },
        { ItemType.Equippable, new EquippableItemFactory() }
    };

    public static ItemData CreateItem(ItemType type, ItemData data)
    {
        if (FactoryMap.TryGetValue(type, out IItemFactory factory))
        {
            return factory.CreateItem(data);
        }
        throw new ArgumentException($"Unknown item type: {type}");
    }
}