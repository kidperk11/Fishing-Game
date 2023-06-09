using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SocketInformation
{
    public ItemSocket socket;
    public bool isConnected;
    public ItemSocket connection;
}

public class SocketDragItem : MonoBehaviour, IRaycastable
{
    public SocketInformation[] socketsInfo;

    [SerializeField] public GameObject[] grabLocations;

    [Space(10)]
    [SerializeField] int gravityScale;
    [SerializeField] int spriteStartIndex;
    
    [Header("Components")]
    [SerializeField] SpriteSheetUpdate hasSpriteSheet;
    [SerializeField] Rigidbody2D rb = null;

    [Header("Collide Events")]
    [SerializeField] UnityEvent beginCollide;
    [SerializeField] UnityEvent afterCollide;

    [HideInInspector] public bool dragItem = false;
    private MousePosition mousePosition;
    private Vector3 currentMousePosition = Vector3.zero;
    private Vector3 dragOffset;
    private GameObject closestGrab;

    private void Awake()
    {
        var inventory = GameObject.FindGameObjectWithTag("InventoryContainer");
        this.mousePosition = inventory.GetComponentInParent<MousePosition>();

        if (hasSpriteSheet != null)
        {
            hasSpriteSheet.currentSprite = spriteStartIndex;
        }
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
    }

    private void DropDraggedItem()
    {
        foreach (SocketInformation socket in socketsInfo)
        {
            socket.socket.isDragging = false;
        }

        afterCollide.Invoke();
        dragItem = false;
        rb.gravityScale = gravityScale;

        if (hasSpriteSheet != null)
        {
            hasSpriteSheet.NewSpriteIndex(0);
        }
    }

    private void PickupDragItem(Vector3 dragPosition)
    {
        foreach (SocketInformation socket in socketsInfo)
        {
            socket.socket.isDragging = true;
        }

        rb.gravityScale = 0f;

        dragPosition = mousePosition.GetMousePosition();

        MoveItemPosition(dragPosition - dragOffset);
        DragConnectedItems(dragPosition - dragOffset);

        if (hasSpriteSheet != null)
        {
            hasSpriteSheet.NewSpriteIndex(1);
        }
    }

    public void MoveItemPosition(Vector3 movePosition)
    {
        rb.MovePosition(movePosition);
    }

    private void DragConnectedItems(Vector3 movePosition)
    {
        foreach(SocketInformation socketInfo in socketsInfo)
        {
            if(socketInfo.isConnected)
            {
                SocketDragItem connectedItem = socketInfo.connection.GetComponentInParent<SocketDragItem>();

                Vector3 connectionOffset = connectedItem.transform.position - transform.position;

                connectedItem.MoveItemPosition(movePosition + connectionOffset);
                
            }
        }
    }    


    public bool HandleRaycast(CursorController callingController)
    {
        if (Input.GetMouseButtonDown(0))
            DragAction();

        return true;
    }

    private void DragAction()
    {
        closestGrab = FindClosestGrabLocation();

        if (!dragItem)
        {
            dragOffset = (closestGrab.transform.position - transform.localPosition);
            dragItem = true;
        }

        beginCollide.Invoke();
    }

    private GameObject FindClosestGrabLocation()
    {
        GameObject closestLocation = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject location in grabLocations)
        {
            float distance = Vector3.Distance(mousePosition.GetMousePosition(), location.transform.position);

            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestLocation = location;
            }
        }

        return closestLocation;
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
