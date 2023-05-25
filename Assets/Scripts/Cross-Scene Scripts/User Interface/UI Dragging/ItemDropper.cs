using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To be placed on anything that wishes to drop pickups into the world.
/// Tracks the drops for saving and restoring.
/// </summary>

public class ItemDropper : MonoBehaviour
{
    public MousePosition mousePosition;

    private List<WorldItemPickup> droppedItems = new List<WorldItemPickup>();
    private List<UIItemPickup> droppedUIItems = new List<UIItemPickup>();



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
        SpawnPickup(item, GetDropLocation(item), number);
    }


    /// <summary>
    /// Create a pickup at the current position.
    /// </summary>
    /// <param name="item">The item type for the pickup.</param>
    public void DropItem(InventoryItem item)
    {
        SpawnPickup(item, GetDropLocation(item), 1);
    }

    /// <summary>
    /// Override to set a custom method for locating a drop.
    /// </summary>
    /// <returns>The location the drop should be spawned.</returns>
    protected virtual Vector3 GetDropLocation(InventoryItem item)
    {
        switch(item.itemPickupType)
        {
            case PickupType.World:
                return transform.position;
            case PickupType.UI:
                return Vector3.zero;
            default:
                return Vector3.zero;
        }
    }

    public void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
    {
        switch(item.itemPickupType)
        {
            case PickupType.World:
            {
                    Vector3 whereToSpawn = mousePosition.GetMousePosition();
                    var pickup = item.SpawnWorldPickup(whereToSpawn, number);
                    droppedItems.Add(pickup);
                    break;
            }
            case PickupType.UI:
            {
                    Vector3 whereToSpawn = mousePosition.GetMousePosition();
                    var pickup = item.SpawnUIPickup(whereToSpawn, number);
                    droppedUIItems.Add(pickup);
                    break;
            }
        }
    }
}
