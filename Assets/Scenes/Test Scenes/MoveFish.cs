using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFish : MonoBehaviour
{
    public GameObject center;
    float currentVelocity;
    public float smoothTime = 1f;
    public float currentSpeed = 0.0f;
    public float targetSpeed = 10f;
    public float radius = 5;
    public Vector3 desiredPosition;

    public bool rotate;
    public float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        //Vector3 directionToSun = center.transform.position - transform.position;
        //Vector3 rotationVector = Quaternion.Euler(0f, currentSpeed * Time.deltaTime, 0f) * directionToSun;
        //transform.position += rotationVector;

        // Accelerate/decelerate towards target speed
        //currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref currentVelocity, smoothTime);

        //transform.RotateAround(center.transform.position, Vector3.up, currentSpeed * Time.deltaTime);

        //Debug.Log((transform.position).normalized);

        //if(rotate)
        //{
        //    Vector3 targetPosition = new Vector3(0, 0, -1);

        //    Vector3 displacement = targetPosition - transform.position;
        //    Vector3 velocity = displacement / desiredTime;

        //    transform.position += velocity * Time.deltaTime;
        //}

        float angularSpeed = speed / radius;

        // Calculate the direction vector
        Vector3 direction = new Vector3(Mathf.Sin(Time.time * angularSpeed), 0f, Mathf.Cos(Time.time * angularSpeed));

        // Set the magnitude of the velocity vector to the desired speed
        Vector3 velocity = direction.normalized * speed;

        transform.position += velocity * Time.deltaTime;
        MoveToNewRadius();
    }

    private void MoveToNewRadius()
    {
        desiredPosition = (transform.position - center.transform.position).normalized * radius + center.transform.position;
        Vector3 newRadius = desiredPosition - transform.position;
        transform.position += newRadius;
    }
}
