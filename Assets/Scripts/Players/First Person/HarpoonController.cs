using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonController : MonoBehaviour
{
    public FireHarpoon harpoonGun;
    public Rigidbody rb;
    public QTETickerController ticker;

    bool enemyHit;
    public float harpoonSpeed;

    // Start is called before the first frame update
    void Start()
    {
        enemyHit = false;
        ticker = harpoonGun.ticker;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit with harpoon");
            enemyHit = true;
            EnemyHealthAndQTE enemyHealth = collision.gameObject.GetComponent<EnemyHealthAndQTE>();
            ticker.ActivateTicker(enemyHealth, this);
            harpoonGun.ActivateReel();
            Destroy(rb);
            transform.parent = collision.gameObject.transform;
        }

        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Failed Hit on: " + collision.gameObject.tag + ". Harpoon will now be destroyed.");
            ResetHarpoon();
        }
    }

    public void ResetHarpoon()
    {
        transform.parent = null;
        harpoonGun.ResetFire();
        harpoonGun.ResetReel();
        Destroy(this.gameObject);
    }
}
