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
    public bool adjust;


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
        adjust = false;
        angle += 9;
    }
}
