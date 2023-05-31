using UnityEngine;

/// <summary>
/// To be placed at the root of a Pickup prefab. Contains the data about the
/// pickup such as the type of item and the number.
/// </summary> 

public class UIItemPickup : MonoBehaviour, IRaycastable
{
    [SerializeField] InventoryItem item = null;
    [SerializeField] Rigidbody rb = null;
    [SerializeField] CursorSpeed cursorSpeed;
    public bool clickPickup = true;
    int number = 1;

    private InventoryController inventory;
    private MousePosition mousePosition;
    private bool dragItem = false;
    public float throwForce;
    public int gravityScale;
    public int spriteStartIndex;
    public SpriteSheetUpdate hasSpriteSheet;



    public Vector3 lastMousePos;
    public Vector3 mouseDelta;




    private void Awake()
    {
        var inventory = GameObject.FindGameObjectWithTag("InventoryContainer");
        this.inventory = inventory.GetComponent<InventoryController>();
        this.mousePosition = inventory.GetComponentInParent<MousePosition>();

        if(hasSpriteSheet != null)
        {
            hasSpriteSheet.currentSprite = spriteStartIndex;
        }
    }

    private void Start()
    {
        lastMousePos = Input.mousePosition;
    }

    private void Update()
    {
        Vector3 currentMousePosition = Vector3.zero;

        //Left mouse button was released. dragItem is not longer true. Turn on gravity.
        if (dragItem && Input.GetMouseButtonUp(0))
        {
            mouseDelta = Input.mousePosition - lastMousePos;
            rb.AddForce(mouseDelta * throwForce, ForceMode.Impulse);

            dragItem = false;
            rb.useGravity = true;
            if (hasSpriteSheet != null)
            {
                hasSpriteSheet.NewSpriteIndex(0);
            }
        }
        //Left Mouse button is being held down. Continue to drag
        else if (dragItem)
        {


            rb.useGravity = false;
            currentMousePosition = mousePosition.GetMousePosition();
            rb.MovePosition(currentMousePosition);

            if (hasSpriteSheet != null)
            {
                hasSpriteSheet.NewSpriteIndex(1);
            }
        }

        lastMousePos = Input.mousePosition;
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
