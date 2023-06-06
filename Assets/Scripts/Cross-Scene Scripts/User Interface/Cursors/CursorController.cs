using UnityEngine;
using System;
using UnityEngine.EventSystems;

public enum CursorType
{
    None,
    UI,
    Point,
    Open,
    Grab,
}

public class CursorController : MonoBehaviour
{
    [System.Serializable]
    struct CursorMapping
    {
        public string cursorName;
        public CursorType type;
        public Texture2D texture;
        public Vector2 hotspot;
    }

    [SerializeField] CursorMapping[] cursorMappings = null;
    [SerializeField] float raycastRadius = 1f;

    public bool useCursors = false;
    bool isDraggingUI = false;

    private void Update()
    {
        if (InteractWithUI()) return;

        if (InteractWithComponent2D()) return;
        if (InteractWithComponent3D()) return;

        //SetCursor(CursorType.None);
        
    }

    private bool InteractWithUI()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isDraggingUI = false;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {

            if (Input.GetMouseButtonDown(0))
            {
                isDraggingUI = true;
            }

            SetCursor(CursorType.UI);
            
            return true;
        }

        if (isDraggingUI)
        {
            return true;
        }

        return false;
    }

    private bool InteractWithComponent2D()
    {
        RaycastHit2D[] hits = RaycastAllSorted2D();

        foreach (RaycastHit2D hit in hits)
        {
            IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

            foreach (IRaycastable raycastable in raycastables)
            {
                raycastable.HandleRaycast(this);
            }
        }
        return false;
    }

    private bool InteractWithComponent3D()
    {
        RaycastHit[] hits = RaycastAllSorted3D();

        foreach (RaycastHit hit in hits)
        {
            IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

            foreach (IRaycastable raycastable in raycastables)
            {
                raycastable.HandleRaycast(this);

                if (raycastable.HandleRaycast(this))
                {
                    SetCursor(raycastable.GetCursorType());
                    return true;
                }
            }
        }
        return false;
    }

    RaycastHit2D[] RaycastAllSorted2D()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(GetMousePosition(), 0.5f, Vector2.zero);

        float[] distances = new float[hits.Length];
        for (int i = 0; i < hits.Length; i++)
        {
            distances[i] = hits[i].distance;
        }
        Array.Sort(distances, hits);
        return hits;
    }

    RaycastHit[] RaycastAllSorted3D()
    {
        RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);

        float[] distances = new float[hits.Length];
        for (int i = 0; i < hits.Length; i++)
        {
            distances[i] = hits[i].distance;
        }
        Array.Sort(distances, hits);
        return hits;
    }

    private void SetCursor(CursorType type)
    {
        if(useCursors)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }
        
    }

    private CursorMapping GetCursorMapping(CursorType type)
    {
        foreach (CursorMapping mapping in cursorMappings)
        {
            if (mapping.type == type)
            {
                return mapping;
            }
        }
        return cursorMappings[0];
    }

    private static Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    private static Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
