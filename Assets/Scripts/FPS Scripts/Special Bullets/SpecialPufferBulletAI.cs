using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpecialPufferBulletAI : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody rb;
    public GameObject damageZone;


    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")){
            //NOTE: Add code for an explosion animation, potentially a coroutine 
            Destroy(rb);
            damageZone.SetActive(true);
            Destroy(this.gameObject, 3);
        }
    }


}
