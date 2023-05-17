using UnityEngine;

[CreateAssetMenu(menuName = ("User Interface/World Pickup Item"))]
public class InventoryItem : ScriptableObject
{

    [Tooltip("Item name to be displayed in UI.")]
    [SerializeField] string displayName = null;
    [Tooltip("Item description to be displayed in UI.")]
    [SerializeField][TextArea] string description = null;
    [Tooltip("The UI icon to represent this item in the inventory.")]
    [SerializeField] Sprite icon = null;
    [Tooltip("The prefab that should be spawned when this item is dropped.")]
    public Pickup pickup = null;
    [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
    [SerializeField] bool stackable = false;

    [Tooltip("Where are we allowed to put this item.")]
    [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.A;

    public Pickup SpawnPickup(Vector3 position, int number)
    {
        var pickup = Instantiate(this.pickup);
        pickup.transform.position = position;
        pickup.Setup(this, number);
        return pickup;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public bool IsStackable()
    {
        return stackable;
    }
        
    public string GetDisplayName()
    {
        return displayName;
    }

    public string GetDescription()
    {
        return description;
    }

    public EquipLocation GetAllowedEquipLocation()
    {
        return allowedEquipLocation;
    }
}
