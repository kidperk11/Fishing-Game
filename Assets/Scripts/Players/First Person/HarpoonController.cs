using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonController : MonoBehaviour
{
    public FireHarpoon harpoonGun;
    public Rigidbody rb;

    bool enemyHit;
    public float harpoonSpeed;

    // Start is called before the first frame update
    void Start()
    {
        enemyHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!enemyHit)
        //{
        //    rb.velocity = Vector3.forward * harpoonSpeed;
        //}
        //else
        //{
        //    rb.velocity = new Vector3(0, 0, 0);
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit with harpoon");
            enemyHit = true;
            Destroy(rb);
            transform.parent = collision.gameObject.transform;
        }

        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Failed Hit on: " + collision.gameObject.tag + ". Harpoon will now be destroyed.");
            harpoonGun.ResetFire();
            Destroy(this.gameObject);
        }
    }
}
