using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To be placed on anything that wishes to drop pickups into the world.
/// Tracks the drops for saving and restoring.
/// </summary>

public class ItemDropper : MonoBehaviour
{

    private List<Pickup> droppedItems = new List<Pickup>();



    /// <summary>
    /// Create a pickup at the current position.
    /// </summary>
    /// <param name="item">The item type for the pickup.</param>
    /// <param name="number">
    /// The number of items contained in the pickup. Only used if the item
    /// is stackable.
    /// </param>
    public void DropItem(InventoryItem item, int number)
    {
        SpawnPickup(item, GetDropLocation(), number);
    }


    /// <summary>
    /// Create a pickup at the current position.
    /// </summary>
    /// <param name="item">The item type for the pickup.</param>
    public void DropItem(InventoryItem item)
    {
        SpawnPickup(item, GetDropLocation(), 1);
    }

    /// <summary>
    /// Override to set a custom method for locating a drop.
    /// </summary>
    /// <returns>The location the drop should be spawned.</returns>
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
