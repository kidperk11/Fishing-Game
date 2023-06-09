﻿using UnityEngine;

/// <summary>
/// To be placed at the root of a Pickup prefab. Contains the data about the
/// pickup such as the type of item and the number.
/// </summary> 

public class WorldItemPickup3D : MonoBehaviour, IRaycastable
{
    [SerializeField] InventoryItem3D item = null;
    int number = 1;

    InventoryController inventory;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("InventoryContainer");
        inventory = player.GetComponent<InventoryController>();
    }

    public void Setup(InventoryItem3D item, int number)
    {
        this.item = item;
        if (!item.IsStackable())
        {
            number = 1;
        }
        this.number = number;
    }

    public InventoryItem3D GetItem()
    {
        return item;
    }

    public int GetNumber()
    {
        return number;
    }

    public void PickupItem()
    {
        bool foundSlot = inventory.AddToFirstEmptySlot(item, number);
        if (foundSlot)
        {
            Destroy(gameObject);
        }
    }

    public bool CanBePickedUp()
    {
        return inventory.HasSpaceFor(item);
    }

    public bool HandleRaycast(CursorController callingController)
    {
        if (Input.GetMouseButtonDown(0))
        {
            PickupItem();
        }
        return true;
    }

    public CursorType GetCursorType()
    {
        return CursorType.Point;
    }
}