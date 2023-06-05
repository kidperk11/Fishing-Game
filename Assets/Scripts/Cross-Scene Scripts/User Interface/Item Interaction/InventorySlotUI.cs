using UnityEngine;


public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem3D>
{
    [SerializeField] InventoryItemIcon icon = null;
    [SerializeField] EquipLocation equipLocation = EquipLocation.A;

    int index;
    InventoryController inventory;


    private void Awake()
    {
        if (equipLocation != EquipLocation.Inventory)
        {
            var player = GameObject.FindGameObjectWithTag("InventoryContainer");
            inventory = player.GetComponent<InventoryController>();
            inventory.equipmentUpdated += RedrawUI;
            return;
        }
    }

    private void Start()
    {
        if (equipLocation != EquipLocation.Inventory)
        {
            RedrawUI();
        }
    }

    public void Setup(InventoryController inventory, int index)
    {
        this.inventory = inventory;
        this.index = index;
        icon.SetItem(inventory.GetItemInSlot3D(index), inventory.GetNumberInSlot3D(index));
    }

    public int MaxAcceptable(InventoryItem3D item)
    {
        if (equipLocation == EquipLocation.Inventory)
        {
            if (inventory.HasSpaceFor(item))
            {
                return int.MaxValue;
            }
            return 0;

        }
        else
        {
            InventoryItem3D equipableItem = item as InventoryItem3D;
            if (equipableItem == null) return 0;
            if (equipableItem.GetAllowedEquipLocation() != equipLocation) return 0;
            if (GetItem() != null) return 0;

            return 1;
        }
    }

    public void AddItems(InventoryItem3D item, int number)
    {
        if (equipLocation == EquipLocation.Inventory)
        {
            inventory.AddItemToSlot3D(index, item, number);
        }
        else
        {
            inventory.EquipmentAddItem(equipLocation, (InventoryItem3D)item);
        }
    }

    public InventoryItem3D GetItem()
    {
        if (equipLocation == EquipLocation.Inventory)
        {
            return inventory.GetItemInSlot3D(index);
        }
        else
        {
            return inventory.GetItemInSlot3D(equipLocation);
        }
    }

    public int GetNumber()
    {
        if (equipLocation == EquipLocation.Inventory)
        {
            return inventory.GetNumberInSlot3D(index);
        }
        else
        {
            if (GetItem() != null)
                return 1;
            else
                return 0;
        }
    }

    public void RemoveItems(int number)
    {
        if (equipLocation == EquipLocation.Inventory)
        {
            inventory.RemoveFromSlot3D(index, number);
        }
        else
        {
            inventory.EquipmentRemoveItem3D(equipLocation);
        }
    }
     
    void RedrawUI()
    {
        icon.SetItem(inventory.GetItemInSlot3D(equipLocation));
    }
}
