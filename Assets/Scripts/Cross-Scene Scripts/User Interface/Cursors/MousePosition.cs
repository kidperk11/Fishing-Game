using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    public Camera mainCamera;

    private Vector3 mousePosition;
    public Vector3 WorldPosition;

    public GameObject UIPrefab;
    public GameObject UIPrefabTransform;

    private void Update()
    {
        GetMousePosition();

        //GetWorldMousePosition();

        if (Input.GetMouseButtonDown(0))
        {
            SpawnUIElement();
        }
    }

    private void GetWorldMousePosition()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 0;
    }

    private void GetMousePosition()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(mousePosition);
    }

    private void SpawnUIElement()
    {
        GameObject newUIElement = Instantiate(UIPrefab, WorldPosition, Quaternion.identity) as GameObject;
        newUIElement.transform.SetParent(UIPrefabTransform.transform, false);
    }
}
