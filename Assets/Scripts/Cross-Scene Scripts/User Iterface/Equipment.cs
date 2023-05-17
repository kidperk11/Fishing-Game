using System;
using System.Collections.Generic;
using UnityEngine;

public enum SlotType
{
    Inventory,
    Equipment,
}

public enum EquipLocation
{
    A,
    B,
    C,
    D,
    All,
}

public class Equipment : MonoBehaviour
{
    Dictionary<EquipLocation, InventoryItem> equippedItems = new Dictionary<EquipLocation, InventoryItem>();

    public event Action equipmentUpdated;

    public InventoryItem GetItemInSlot(EquipLocation equipLocation)
    {
        if (!equippedItems.ContainsKey(equipLocation))
        {
            return null;
        }

        return equippedItems[equipLocation];
    }

    public void AddItem(EquipLocation slot, InventoryItem item)
    {
        Debug.Assert(item.GetAllowedEquipLocation() == slot);

        equippedItems[slot] = item;

        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
    }

    public void RemoveItem(EquipLocation slot)
    {
        equippedItems.Remove(slot);
        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
    }

    public IEnumerable<EquipLocation> GetAllPopulatedSlots()
    {
        return equippedItems.Keys;
    }
}