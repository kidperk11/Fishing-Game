using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MouseSpeedType
{
    GetAxis,
    MousePosition,
}
public class CursorSpeed : MonoBehaviour
{
    public MouseSpeedType speedType;

    public Vector3 lastMousePos;
    public Vector3 mouseDelta
    {
        get
        {
            return Input.mousePosition - lastMousePos;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lastMousePos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {

        lastMousePos = Input.mousePosition;

        if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) && speedType == MouseSpeedType.GetAxis)
            Debug.Log(string.Format("Mouse X: {0} Mouse Y: {1}", Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
        else
        {
            //Debug.Log(string.Format("Mouse Vector3: {0}", mouseDelta));

            
        }
    }
}
