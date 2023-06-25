using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float speed = 1000f;
    public GameObject parentBody;

    private Rigidbody rb;
    public float initialVelocity = 10f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SetVelocity();

    }


    void FixedUpdate()
    {
        SetVelocity();

        Vector3 dir = (parentBody.transform.position - transform.position).normalized;
        Vector3 cross = Vector3.Cross(Vector3.up, dir).normalized;

        rb.AddForce(dir * (Mathf.Pow(initialVelocity, 2)) / Vector3.Distance(transform.position, parentBody.transform.position));
    }

    private void Update()
    {
        //ChangeSpeed();
    }

    private void SetVelocity()
    {
        Vector3 dir = (parentBody.transform.position - transform.position).normalized;
        Vector3 cross = Vector3.Cross(Vector3.up, dir).normalized;

        rb.velocity = cross * initialVelocity;
    }


}
    /*
    THINKING CORNER

    F = G * (m1 * m2) / r^2
    
    F is the gravitational force
    G is the gravitational constant
    m1 and m2 are the masses of the two objects
    r is the distance between the two objects

    v = sqrt(G * M / r)
    
    v is the velocity of the orbiting object
    G is the gravitational constant
    M is the mass of the central object
    r is the radius of the orbit


    public void GravityForce()
    {
        float radius = Vector3.Distance(transform.position, parentBody.transform.position);

        GetComponent<Rigidbody>().AddForce((parentBody.transform.position - transform.position).normalized *
            ((speed * speed) / (radius * radius)));
    }

    void InitialVelocity()
    {
        float radius = Vector3.Distance(transform.position, parentBody.transform.position);

        transform.LookAt(parentBody.transform);

        GetComponent<Rigidbody>().velocity += transform.right * Mathf.Sqrt((speed * speed) / radius);

    }

    */
