using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    readonly float G = 100f;
    public GameObject parentBody;

    //Start is called before the first frame update
    void Awake()
    {
        InitialVelocity();
    }

    //Update is called once per frame
    void FixedUpdate()
    {
        GravityForce();
    }

    public void GravityForce()
    {
        float m1 = GetComponent<Rigidbody>().mass;
        float m2 = parentBody.GetComponent<Rigidbody>().mass;
        float r = Vector3.Distance(transform.position, parentBody.transform.position);

        GetComponent<Rigidbody>().AddForce((parentBody.transform.position - transform.position).normalized *
            (G * (m1 * m2) / (r*r)));
    }

    void InitialVelocity()
    {
        float m2 = parentBody.GetComponent<Rigidbody>().mass;
        float r = Vector3.Distance(transform.position, parentBody.transform.position);
        transform.LookAt(parentBody.transform);

        GetComponent<Rigidbody>().velocity += transform.right * Mathf.Sqrt((G * m2) / r);
    }
}
