using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{

    private List<Pickup> droppedItems = new List<Pickup>();

    public void DropItem(InventoryItem item, int number)
    {
        SpawnPickup(item, GetDropLocation(), number);
    }

    public void DropItem(InventoryItem item)
    {
        SpawnPickup(item, GetDropLocation(), 1);
    }

    protected virtual Vector3 GetDropLocation()
    {
        return transform.position;
    }

    public void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
    {
        var pickup = item.SpawnPickup(spawnLocation, number);
        droppedItems.Add(pickup);
    }
}
