using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPufferBulletAI : MonoBehaviour
{
    public float bulletSpeed;
    [SerializeField] private int bulletDamage;
    public Rigidbody rb;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //rb.AddForce(Vector3.forward * bulletSpeed, ForceMode.Impulse);
    }

    private void Update()
    {
        transform.LookAt(player.transform);
    }

    void FixedUpdate()
    {
        //rb.velocity = Vector3.forward * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<FPPlayerHealth>().TakeDamage(bulletDamage);
            Destroy(this.gameObject);
        }
        else if (!other.gameObject.CompareTag("Enemy")) { Destroy(this.gameObject); }
    }
}
