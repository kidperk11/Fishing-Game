using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MousePosition : MonoBehaviour
{
    public Vector3 newScreenPosition;
    public Vector3 worldPosition;
    public LayerMask layersToHit;

    public Vector3 GetMousePosition()
    {
        newScreenPosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(newScreenPosition);
        if (Physics.Raycast(ray, out RaycastHit hitData, 100f, layersToHit))
        {
            worldPosition = hitData.point;
        }

        return worldPosition;
    }
}
