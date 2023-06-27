using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceOnCylinder : MonoBehaviour
{
    public GameObject player;
    public GameObject objectToPlace;

    public float cylinderRadius = 5f;
    public float angle = 45f;
    public bool activate;
    public bool movePlayer;

    private void Update()
    {
        if (activate)
        {
            AddObject();

        }

        if(movePlayer)
        {
            // Convert angle to radians
            float radians = angle * Mathf.Deg2Rad;

            // Calculate position on the circumference
            float x = cylinderRadius * Mathf.Cos(radians);
            float y = 0f;
            float z = cylinderRadius * Mathf.Sin(radians);

            // Set the object's position
            player.transform.position = new Vector3(x, y, z);
        }
    }

    private void AddObject()
    {
        var newSphere = Instantiate(objectToPlace, Vector3.zero, Quaternion.identity);

        // Convert angle to radians
        float radians = angle * Mathf.Deg2Rad;

        // Calculate position on the circumference
        float x = cylinderRadius * Mathf.Cos(radians);
        float y = 0f;
        float z = cylinderRadius * Mathf.Sin(radians);

        // Set the object's position
        newSphere.transform.position = new Vector3(x, y, z);
        activate = false;
    }
}
