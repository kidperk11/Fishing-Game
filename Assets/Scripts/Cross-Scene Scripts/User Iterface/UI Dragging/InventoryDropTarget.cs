using UnityEngine;

public class InventoryDropTarget : MonoBehaviour, IDragDestination<InventoryItem>
{
    public void AddItems(InventoryItem item, int number)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<ItemDropper>().DropItem(item, number);
    }

    public int MaxAcceptable(InventoryItem item)
    {
        return int.MaxValue;
    }
}
