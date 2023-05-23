using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MousePosition : MonoBehaviour
{
    private Vector3 m_MousePosition;

    public Vector3 mousePosition { get { return m_MousePosition; } }

    public GameObject UIPrefab;
    public GameObject UIPrefabTransform;

    public bool placeObject;

    private void Update()
    {
        m_MousePosition = Input.mousePosition;
        m_MousePosition.x -= (Screen.width / 2);
        m_MousePosition.y -= (Screen.height / 2);
        m_MousePosition.z = 0;

        if (Input.GetMouseButtonDown(0) && placeObject)
        {
            SpawnUIElement();
        }
    }

    private void SpawnUIElement()
    {
        GameObject newUIElement = Instantiate(UIPrefab, m_MousePosition, Quaternion.identity) as GameObject;
        newUIElement.transform.SetParent(UIPrefabTransform.transform, false);
    }
}
