using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("User Interface/Pickup Item 2D"))]
public class InventoryItem2D : ScriptableObject
{
    [Tooltip("Item name to be displayed in UI.")]
    [SerializeField] string displayName = null;
    [Tooltip("Item description to be displayed in UI.")]
    [SerializeField] [TextArea] string description = null;
    [Tooltip("The UI icon to represent this item in the inventory.")]
    [SerializeField] Sprite icon = null;

    [Tooltip("The prefab that should be spawned when this item is dropped.")]
    public WorldItemPickup2D worldPickup = null;
    public UIItemPickup2D uiPickup = null;

    [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
    [SerializeField] bool stackable = false;

    [Tooltip("Where are we allowed to put this item.")]
    [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.A;
    [SerializeField] public PickupType itemPickupType;
    private Transform parentObject;

    public WorldItemPickup2D SpawnWorldPickup(Vector3 position, int number)
    {
        var pickup = Instantiate(this.worldPickup);
        pickup.transform.position = position;
        pickup.Setup(this, number);
        return pickup;
    }

    public UIItemPickup2D SpawnUIPickup(Vector3 position, int number)
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
