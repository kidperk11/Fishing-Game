using UnityEngine;


public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
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
        icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
    }

    public int MaxAcceptable(InventoryItem item)
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
            InventoryItem equipableItem = item as InventoryItem;
            if (equipableItem == null) return 0;
            if (equipableItem.GetAllowedEquipLocation() != equipLocation) return 0;
            if (GetItem() != null) return 0;

            return 1;
        }
    }

    public void AddItems(InventoryItem item, int number)
    {
        if (equipLocation == EquipLocation.Inventory)
        {
            inventory.AddItemToSlot(index, item, number);
        }
        else
        {
            inventory.EquipmentAddItem(equipLocation, (InventoryItem)item);
        }
    }

    public InventoryItem GetItem()
    {
        if (equipLocation == EquipLocation.Inventory)
        {
            return inventory.GetItemInSlot(index);
        }
        else
        {
            return inventory.GetItemInSlot(equipLocation);
        }
    }

    public int GetNumber()
    {
        if (equipLocation == EquipLocation.Inventory)
        {
            return inventory.GetNumberInSlot(index);
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
            inventory.RemoveFromSlot(index, number);
        }
        else
        {
            inventory.EquipmentRemoveItem(equipLocation);
        }
    }
     
    void RedrawUI()
    {
        icon.SetItem(inventory.GetItemInSlot(equipLocation));
    }
}
