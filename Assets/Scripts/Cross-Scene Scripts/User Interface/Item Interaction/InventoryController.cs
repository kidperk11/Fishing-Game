using System;
using System.Collections.Generic;
using UnityEngine;

#region Enums

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

#endregion

public class InventoryController : MonoBehaviour
{
#region Inventory
    [Tooltip("Allowed size")]
    [SerializeField] int inventorySize = 16;

    InventorySlot[] slots;

    public struct InventorySlot
    {
        public InventoryItem item;
        public int number;
    }

    public event Action inventoryUpdated;

    public static InventoryController GetPlayerInventory()
    {
        var player = GameObject.FindWithTag("InventoryContainer");
        return player.GetComponent<InventoryController>();
    }

    public bool HasSpaceFor(InventoryItem item)
    {
        return FindSlot(item) >= 0;
    }

    public int GetSize()
    {
        return slots.Length;
    }

    public bool AddToFirstEmptySlot(InventoryItem item, int number)
    {
        int i = FindSlot(item);

        if (i < 0)
        {
            return false;
        }

        slots[i].item = item;
        slots[i].number += number;
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        return true;
    }

    public bool HasItem(InventoryItem item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i].item, item))
            {
                return true;
            }
        }
        return false;
    }

    public InventoryItem GetItemInSlot(int slot)
    {
        return slots[slot].item;
    }

    public int GetNumberInSlot(int slot)
    {
        return slots[slot].number;
    }

    public void RemoveFromSlot(int slot, int number)
    {
        slots[slot].number -= number;
        if (slots[slot].number <= 0)
        {
            slots[slot].number = 0;
            slots[slot].item = null;
        }
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
    }

    public bool AddItemToSlot(int slot, InventoryItem item, int number)
    {
        if (slots[slot].item != null)
        {
            return AddToFirstEmptySlot(item, number); ;
        }

        var i = FindStack(item);
        if (i >= 0)
        {
            slot = i;
        }

        slots[slot].item = item;
        slots[slot].number += number;
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        return true;
    }

    private void Awake()
    {
        slots = new InventorySlot[inventorySize];
    }

    private int FindSlot(InventoryItem item)
    {
        int i = FindStack(item);
        if (i < 0)
        {
            i = FindEmptySlot();
        }
        return i;
    }

    private int FindEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return i;
            }
        }
        return -1;
    }

    private int FindStack(InventoryItem item)
    {
        if (!item.IsStackable())
        {
            return -1;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i].item, item))
            {
                return i;
            }
        }
        return -1;
    }
#endregion

#region Equipment

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

    public void EquipmentAddItem(EquipLocation slot, InventoryItem item)
    {
        Debug.Assert(item.GetAllowedEquipLocation() == slot);

        equippedItems[slot] = item;

        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
    }

    public void EquipmentRemoveItem(EquipLocation slot)
    {
        equippedItems.Remove(slot);
        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
    }

#endregion
}
