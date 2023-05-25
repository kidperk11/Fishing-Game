using UnityEngine;

/// <summary>
/// To be placed at the root of a Pickup prefab. Contains the data about the
/// pickup such as the type of item and the number.
/// </summary> 

public class UIItemPickup : MonoBehaviour, IRaycastable
{
    [SerializeField] InventoryItem item = null;
    public bool clickPickup = true;
    int number = 1;

    private InventoryController inventory;
    private MousePosition mousePosition;
    private bool dragItem = false;

    private void Awake()
    {
        var inventory = GameObject.FindGameObjectWithTag("InventoryContainer");
        this.inventory = inventory.GetComponent<InventoryController>();
        this.mousePosition = inventory.GetComponentInParent<MousePosition>();
    }

    private void Update()
    {

        if (dragItem && Input.GetMouseButtonUp(0))
        {
            dragItem = false;
        }
        else if (dragItem)
        {
            Vector3 currentMousePosition = mousePosition.GetMousePosition();
            transform.position = currentMousePosition;
        }
    
    }

    public void Setup(InventoryItem item, int number)
    {
        this.item = item;
        if (!item.IsStackable())
        {
            number = 1;
        }
        this.number = number;
    }

    public InventoryItem GetItem()
    {
        return item;
    }

    public int GetNumber()
    {
        return number;
    }

    public void PickupItem()
    {
        bool foundSlot = inventory.AddToFirstEmptySlot(item, number);
        if (foundSlot)
        {
            Destroy(gameObject);
        }
    }

    public bool CanBePickedUp()
    {
        return inventory.HasSpaceFor(item);
    }

    public bool HandleRaycast(CursorController callingController)
    {
        if (Input.GetMouseButtonDown(0))
            ItemAction();
        
        return true;
    }

    private void ItemAction()
    {
        if (clickPickup)
        {
            PickupItem();
        }
        else
        {
            dragItem = true;
        }
    }

    public CursorType GetCursorType()
    {
        return CursorType.Pickup;
    }
}
