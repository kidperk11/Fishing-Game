using UnityEngine;


public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
{
    [SerializeField] InventoryItemIcon icon = null;
    [SerializeField] EquipLocation equipLocation = EquipLocation.A;
    [SerializeField] SlotType slotType = SlotType.Inventory;

    int index;
    InventoryItem item;
    Inventory inventory;
    Equipment playerEquipment;


    private void Awake()
    {
        if (slotType == SlotType.Equipment)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            playerEquipment = player.GetComponent<Equipment>();
            playerEquipment.equipmentUpdated += RedrawUI;
        }
    }

    private void Start()
    {
        if (slotType == SlotType.Equipment)
            RedrawUI();
    }

    public void Setup(Inventory inventory, int index)
    {
        this.inventory = inventory;
        this.index = index;
        icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
    }

    public int MaxAcceptable(InventoryItem item)
    {
        if (slotType == SlotType.Equipment)
        {
            InventoryItem equipableItem = item as InventoryItem;
            if (equipableItem == null) return 0;
            if (equipableItem.GetAllowedEquipLocation() != equipLocation) return 0;
            if (GetItem() != null) return 0;

            return 1;
        }
        else
        {
            if (inventory.HasSpaceFor(item))
            {
                return int.MaxValue;
            }
            return 0;
        }
    }

    public void AddItems(InventoryItem item, int number)
    {
        if (slotType == SlotType.Equipment)
            playerEquipment.AddItem(equipLocation, (InventoryItem)item);
        else
            inventory.AddItemToSlot(index, item, number);
    }

    public InventoryItem GetItem()
    {
        if (slotType == SlotType.Equipment)
        {
            return playerEquipment.GetItemInSlot(equipLocation);
        }
        else
        {
            return inventory.GetItemInSlot(index);
        }

    }

    public int GetNumber()
    {
        if (slotType == SlotType.Equipment)
        {
            if (GetItem() != null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return inventory.GetNumberInSlot(index);
        }
    }

    public void RemoveItems(int number)
    {
        if (slotType == SlotType.Equipment)
            playerEquipment.RemoveItem(equipLocation);
        else
            inventory.RemoveFromSlot(index, number);
    }

    void RedrawUI()
    {
        icon.SetItem(playerEquipment.GetItemInSlot(equipLocation));
    }
}
