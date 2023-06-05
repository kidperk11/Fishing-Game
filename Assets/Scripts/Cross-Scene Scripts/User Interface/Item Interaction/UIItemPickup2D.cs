using UnityEngine;

/// <summary>
/// To be placed at the root of a Pickup prefab. Contains the data about the
/// pickup such as the type of item and the number.
/// </summary> 

public class UIItemPickup2D : MonoBehaviour, IRaycastable
{
    [SerializeField] InventoryItem2D item = null;
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] CursorSpeed cursorSpeed;
    [SerializeField] bool clickPickup = true;

    [SerializeField] float throwForce;
    [SerializeField] int gravityScale;
    [SerializeField] int spriteStartIndex;
    [SerializeField] SpriteSheetUpdate hasSpriteSheet;

    private InventoryController inventory;
    private MousePosition mousePosition;
    private int number = 1;
    private bool dragItem = false;
    private Vector3 lastMousePos;
    private Vector3 mouseDelta;
    private Vector3 currentMousePosition = Vector3.zero;

    private void Awake()
    {
        var inventory = GameObject.FindGameObjectWithTag("InventoryContainer");
        this.inventory = inventory.GetComponent<InventoryController>();
        this.mousePosition = inventory.GetComponentInParent<MousePosition>();

        if (hasSpriteSheet != null)
        {
            hasSpriteSheet.currentSprite = spriteStartIndex;
        }
    }

    private void OnEnable()
    {
        if (cursorSpeed)
        {
            cursorSpeed = GameObject.FindGameObjectWithTag("InventoryContainer").GetComponentInParent<CursorSpeed>();
        }

        lastMousePos = Input.mousePosition;
    }

    private void Update()
    {

        //Left mouse button was released. dragItem is not longer true. Turn on gravity.
        if (dragItem && Input.GetMouseButtonUp(0))
        {
            DropDraggedItem();
        }
        //Left Mouse button is being held down. Continue to drag
        else if (dragItem)
        {
            PickupDragItem(currentMousePosition);
        }

        lastMousePos = Input.mousePosition;
    }

    private void DropDraggedItem()
    {
        mouseDelta = Input.mousePosition - lastMousePos;
        rb.AddForce(mouseDelta * throwForce, ForceMode2D.Impulse);

        dragItem = false;
        rb.gravityScale = 1f;
        //rb.useGravity = true;
        if (hasSpriteSheet != null)
        {
            hasSpriteSheet.NewSpriteIndex(0);
        }
    }

    private void PickupDragItem(Vector3 dragPosition)
    {
        rb.gravityScale = 0f;
        dragPosition = mousePosition.GetMousePosition();
        rb.MovePosition(dragPosition);

        if (hasSpriteSheet != null)
        {
            hasSpriteSheet.NewSpriteIndex(1);
        }
    }

    public void Setup(InventoryItem2D item, int number)
    {
        this.item = item;
        if (!item.IsStackable())
        {
            number = 1;
        }
        this.number = number;
    }

    public InventoryItem2D GetItem()
    {
        return item;
    }

    public int GetNumber()
    {
        return number;
    }

    public void PickupItem()
    {
        Debug.Log("Pick Up for 2D objects not yet implemented");
        //bool foundSlot = inventory.AddToFirstEmptySlot(item, number);
        //if (foundSlot)
        //{
        //    Destroy(gameObject);
        //}
    }

    public bool CanBePickedUp()
    {
        return inventory.HasSpaceFor(item);
    }

    public bool HandleRaycast(CursorController callingController)
    {
        if (Input.GetMouseButtonDown(0))
            Debug.Log("Touched");
            //ItemAction();

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
        if (dragItem)
        {

            return CursorType.Grab;
        }
        else
        {
            return CursorType.Open;
        }
    }

}
