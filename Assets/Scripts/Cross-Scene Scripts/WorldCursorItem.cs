using UnityEngine;

public class WorldCursorItem : MonoBehaviour, IRaycastable
{ 
    public CursorType GetCursorType()
    {
        return CursorType.Pickup;
    }

    public bool HandleRaycast(CursorController callingController)
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Item Interacted");
        }

        return true;
    }
}