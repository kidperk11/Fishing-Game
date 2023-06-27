using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHEnemyManager : MonoBehaviour
{
    public GameObject plainEnemy;
    public Transform center;
    public float radius = 2.0f;
    public float angle = -90f;

    private float currentSpeed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddEnemy();
        }

        //Circle circumference = 2*PI*radius

        // Rotate player around center
        //transform.RotateAround(center.position, Vector3.up, -currentSpeed * Time.deltaTime);
    }

    private void AddEnemy()
    {
        var newEnemy = Instantiate(plainEnemy, Vector3.zero, Quaternion.identity);

        // Convert angle to radians
        float radians = angle * Mathf.Deg2Rad;

        // Calculate position on the circumference
        float x = radius * Mathf.Cos(radians);
        float y = 0f;
        float z = radius * Mathf.Sin(radians);

        // Set the object's position
        newEnemy.transform.position = new Vector3(x, y, z);
        newEnemy.GetComponent<BHEnemyController>().center = transform;
    }
}
