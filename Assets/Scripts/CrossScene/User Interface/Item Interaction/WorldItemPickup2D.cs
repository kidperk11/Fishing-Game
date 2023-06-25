using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItemPickup2D : MonoBehaviour
{
    [SerializeField] InventoryItem2D item = null;
    int number = 1;

    InventoryController inventory;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("InventoryContainer");
        inventory = player.GetComponent<InventoryController>();
    }

    public void Setup(InventoryItem2D item, int number)
    {
        this.item = item;
        if (!item.IsStackable())
        {
            number = 1;
        }
        this.number = number;
    }

    public InventoryItem2D GetItem()
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
