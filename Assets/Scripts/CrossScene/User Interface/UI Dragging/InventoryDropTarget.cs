using UnityEngine;

/// Handles spawning pickups when item dropped into the world.
/// 
/// Must be placed on the root canvas where items can be dragged. Will be
/// called if dropped over empty space. 

public class InventoryDropTarget : MonoBehaviour, IDragDestination<InventoryItem3D>
{
    public void AddItems(InventoryItem3D item, int number)
    {
        var player = GameObject.FindGameObjectWithTag("InventoryContainer");
        player.GetComponentInParent<ItemDropper>().DropItem(item, number);
    }

    public int MaxAcceptable(InventoryItem3D item)
    {
        return int.MaxValue;
    }
}
