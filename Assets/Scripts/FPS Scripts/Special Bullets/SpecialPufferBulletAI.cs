using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpecialPufferBulletAI : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody rb;
    public GameObject damageZone;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
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
