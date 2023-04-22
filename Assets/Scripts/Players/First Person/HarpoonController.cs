using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonController : MonoBehaviour
{
    public FireHarpoon harpoonGun;
    public float harpoonRange = 100;
    public Vector3 initialPlayerPosition;
    public Rigidbody rb;
    public SphereCollider sphereCollider;
    public QTETickerController ticker;
    private bool touchingPlayer;
    private Collision playerCollision;
    private bool reelSuccess;
    private bool reelFail;
    private EnemyHealthAndQTE hitEnemy;

    [SerializeField] private Vector3 startScale;
    [SerializeField] private Vector3 endScale;

    [SerializeField] private float maxReelTime;
    private float reelTimer;
    private Transform startPoint;
    private Transform endPoint;

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
        endPoint = harpoonGun.harpoonSpawnPoint;
        if (reelSuccess)
        {
            reelTimer += Time.deltaTime;
            float percentageComplete = reelTimer / maxReelTime;
            this.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, percentageComplete);
            hitEnemy.transform.localScale = Vector3.Lerp(startScale, endScale, percentageComplete);
            hitEnemy.transform.position = this.transform.position;
            if (this.transform.position == endPoint.position)
            {
                //NOTE: Consider making a lerp for the enemy as well to make them scale down in size while flying at the player so 
                //the player's FOV is not obstructed

                //NOTE: Add a function for Fish-O-Pedia logging

                harpoonGun.ResetFire();
                harpoonGun.ResetReel();
                Destroy(hitEnemy.gameObject);
                hitEnemy = null;
                Destroy(this.gameObject);
            }
        }

        if (reelFail)
        {
            reelTimer += Time.deltaTime;
            float percentageComplete = reelTimer / maxReelTime;
            this.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, percentageComplete);
            if (this.transform.position == endPoint.position)
            {
                harpoonGun.ResetFire();
                harpoonGun.ResetReel();
                Destroy(this.gameObject);
            }
        }
        if(Vector3.Distance(initialPlayerPosition, this.transform.position) >= harpoonRange)
        {
            ResetHarpoon(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            rb.velocity = Vector3.zero;
            Debug.Log("Enemy hit with harpoon");
            enemyHit = true;
            EnemyHealthAndQTE enemyHealth = collision.gameObject.GetComponent<EnemyHealthAndQTE>();
            hitEnemy = enemyHealth;
            ticker.ActivateTicker(enemyHealth, this);
            harpoonGun.ActivateReel();
            Destroy(rb);
            
            transform.parent = collision.gameObject.transform;
        }

        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Failed Hit on: " + collision.gameObject.tag + ". Harpoon will now be destroyed.");
            ResetHarpoon(false);
        }
    }

    public void ResetHarpoon(bool caughtEnemy)
    {
        transform.parent = null;
        if (caughtEnemy)
        {
            hitEnemy.boxCollider.enabled = false;
            startPoint = this.transform;
            //harpoonGun.ResetFire();
            //harpoonGun.ResetReel();
            //Destroy(this.gameObject);
            reelSuccess = true;
        }
        else 
        {
            startPoint = this.transform;
            reelFail = true; 
        }
        
    }
}
