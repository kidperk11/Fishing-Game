using System;
using System.Collections.Generic;
using UnityEngine;



public enum EquipLocation
{
    A,
    B,
    C,
    D,
    Any,
    Inventory,
}



public class InventoryController : MonoBehaviour
{
#region Inventory
    [Tooltip("Allowed size")]
    [SerializeField] int inventorySize = 16;

    InventorySlot2D[] slots2D;
    InventorySlot3D[] slots3D;

    public struct InventorySlot2D
    {
        public InventoryItem2D item;
        public int number;
    }

    public struct InventorySlot3D
    {
        public InventoryItem3D item;
        public int number;
    }

    public event Action inventoryUpdated;

    public static InventoryController GetPlayerInventory()
    {
        var player = GameObject.FindWithTag("InventoryContainer");
        return player.GetComponentInChildren<InventoryController>();
    }

    public bool HasSpaceFor(InventoryItem2D item)
    {
        return FindSlot(item) >= 0;
    }

    public bool HasSpaceFor(InventoryItem3D item)
    {
        return FindSlot(item) >= 0;
    }

    public int GetSize2D()
    {
        return slots2D.Length;
    }

    public int GetSize3D()
    {
        return slots3D.Length;
    }

    public bool AddToFirstEmptySlot(InventoryItem2D item, int number)
    {
        int i = FindSlot(item);

        if (i < 0)
        {
            return false;
        }

        slots2D[i].item = item;
        slots2D[i].number += number;
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        return true;
    }

    public bool AddToFirstEmptySlot(InventoryItem3D item, int number)
    {
        int i = FindSlot(item);

        if (i < 0)
        {
            return false;
        }

        slots3D[i].item = item;
        slots3D[i].number += number;
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        return true;
    }

    public bool HasItem(InventoryItem2D item)
    {
        for (int i = 0; i < slots2D.Length; i++)
        {
            if (object.ReferenceEquals(slots2D[i].item, item))
            {
                return true;
            }
        }
        return false;
    }

    public bool HasItem(InventoryItem3D item)
    {
        for (int i = 0; i < slots3D.Length; i++)
        {
            if (object.ReferenceEquals(slots3D[i].item, item))
            {
                return true;
            }
        }
        return false;
    }

    public InventoryItem2D GetItemIdnSlot2D(int slot)
    {
        return slots2D[slot].item;
    }

    public InventoryItem3D GetItemInSlot3D(int slot)
    {
        return slots3D[slot].item;
    }

    public int GetNumberInSlot2D(int slot)
    {
        return slots2D[slot].number;
    }

    public int GetNumberInSlot3D(int slot)
    {
        return slots3D[slot].number;
    }

    public void RemoveFromSlot2D(int slot, int number)
    {
        slots2D[slot].number -= number;
        if (slots2D[slot].number <= 0)
        {
            slots2D[slot].number = 0;
            slots2D[slot].item = null;
        }
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
    }

    public void RemoveFromSlot3D(int slot, int number)
    {
        slots3D[slot].number -= number;
        if (slots3D[slot].number <= 0)
        {
            slots3D[slot].number = 0;
            slots3D[slot].item = null;
        }
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
    }

    public bool AddItemToSlot2D(int slot, InventoryItem2D item, int number)
    {
        if (slots2D[slot].item != null)
        {
            return AddToFirstEmptySlot(item, number); ;
        }

        var i = FindStack(item);
        if (i >= 0)
        {
            slot = i;
        }

        slots2D[slot].item = item;
        slots2D[slot].number += number;
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        return true;
    }

    public bool AddItemToSlot3D(int slot, InventoryItem3D item, int number)
    {
        if (slots3D[slot].item != null)
        {
            return AddToFirstEmptySlot(item, number); ;
        }

        var i = FindStack(item);
        if (i >= 0)
        {
            slot = i;
        }

        slots3D[slot].item = item;
        slots3D[slot].number += number;
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        return true;
    }

    private void Awake()
    {
        slots2D = new InventorySlot2D[inventorySize];
        slots3D = new InventorySlot3D[inventorySize];
    }

    private int FindSlot(InventoryItem2D item)
    {
        int i = FindStack(item);
        if (i < 0)
        {
            i = FindEmptySlot2D();
        }
        return i;
    }

    private int FindSlot(InventoryItem3D item)
    {
        int i = FindStack(item);
        if (i < 0)
        {
            i = FindEmptySlot3D();
        }
        return i;
    }

    private int FindEmptySlot2D()
    {
        for (int i = 0; i < slots2D.Length; i++)
        {
            if (slots2D[i].item == null)
            {
                return i;
            }
        }
        return -1;
    }

    private int FindEmptySlot3D()
    {
        for (int i = 0; i < slots3D.Length; i++)
        {
            if (slots3D[i].item == null)
            {
                return i;
            }
        }
        return -1;
    }

    private int FindStack(InventoryItem2D item)
    {
        if (!item.IsStackable())
        {
            return -1;
        }

        for (int i = 0; i < slots2D.Length; i++)
        {
            if (object.ReferenceEquals(slots2D[i].item, item))
            {
                return i;
            }
        }
        return -1;
    }

    private int FindStack(InventoryItem3D item)
    {
        if (!item.IsStackable())
        {
            return -1;
        }

        for (int i = 0; i < slots3D.Length; i++)
        {
            if (object.ReferenceEquals(slots3D[i].item, item))
            {
                return i;
            }
        }
        return -1;
    }

    #endregion

    #region Equipment

    Dictionary<EquipLocation, InventoryItem3D> equippedItems3D = new Dictionary<EquipLocation, InventoryItem3D>();
    Dictionary<EquipLocation, InventoryItem2D> equippedItems2D = new Dictionary<EquipLocation, InventoryItem2D>();
    public event Action equipmentUpdated;

    public InventoryItem2D GetItemInSlot2D(EquipLocation equipLocation)
    {
        if (!equippedItems2D.ContainsKey(equipLocation))
        {
            return null;
        }

        return equippedItems2D[equipLocation];
    }

    public InventoryItem3D GetItemInSlot3D(EquipLocation equipLocation)
    {
        if (!equippedItems3D.ContainsKey(equipLocation))
        {
            return null;
        }

        return equippedItems3D[equipLocation];
    }

    public void EquipmentAddItem(EquipLocation slot, InventoryItem2D item)
    {
        Debug.Assert(item.GetAllowedEquipLocation() == slot);

        equippedItems2D[slot] = item;

        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
    }

    public void EquipmentAddItem(EquipLocation slot, InventoryItem3D item)
    {
        Debug.Assert(item.GetAllowedEquipLocation() == slot);

        equippedItems3D[slot] = item;

        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
    }

    public void EquipmentRefmoveItem2D(EquipLocation slot)
    {
        equippedItems2D.Remove(slot);
        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
    }

    public void EquipmentRemoveItem3D(EquipLocation slot)
    {
        equippedItems3D.Remove(slot);
        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
    }

#endregion
}
