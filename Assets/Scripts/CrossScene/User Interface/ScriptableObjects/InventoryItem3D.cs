using UnityEngine;

public enum PickupType
{
    World,
    UI,
}

[CreateAssetMenu(menuName = ("User Interface/Pickup Item 3D"))]
public class InventoryItem3D : ScriptableObject 
{
    [Tooltip("Item name to be displayed in UI.")]
    [SerializeField] string displayName = null;
    [Tooltip("Item description to be displayed in UI.")]
    [SerializeField][TextArea] string description = null;
    [Tooltip("The UI icon to represent this item in the inventory.")]
    [SerializeField] Sprite icon = null;

    [Tooltip("The prefab that should be spawned when this item is dropped.")]
    public WorldItemPickup3D worldPickup = null;
    public UIItemPickup3D uiPickup = null;

    [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
    [SerializeField] bool stackable = false;

    [Tooltip("Where are we allowed to put this item.")]
    [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.A;
    [SerializeField] public PickupType itemPickupType;
    private Transform parentObject;

    public WorldItemPickup3D SpawnWorldPickup(Vector3 position, int number)
    {
        var pickup = Instantiate(this.worldPickup);
        pickup.transform.position = position;
        pickup.Setup(this, number);
        return pickup;
    }

    public UIItemPickup3D SpawnUIPickup(Vector3 position, int number)
    {
        var pickup = Instantiate(this.uiPickup);
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
