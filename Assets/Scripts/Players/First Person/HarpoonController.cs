using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonController : MonoBehaviour
{
    //Components and Basic Data
    public FireHarpoon harpoonGun;
    public float harpoonRange = 100;
    public Vector3 initialPlayerPosition;
    public Rigidbody rb;
    public SphereCollider sphereCollider;
    public float harpoonSpeed;
    

    //HitEnemyAI
    private EnemyHealthAndQTE hitEnemy;

    //ReelInAI
    [SerializeField] private Vector3 startScale;
    [SerializeField] private Vector3 endScale;
    private float maxReelTime;
    private float reelTimer;
    private Vector3 startPoint;
    private Transform endPoint;

    //HitGrappleAI
    public GameObject grapplePoint;
    public Vector3 grappleTrajectory;
    [SerializeField] private float grappleSpeed;

    //Variables for state machine
    public State state;

    public enum State
    {
        InAir,
        HitEnemy,
        HitItem,
        HitGrapple,
        ReelIn
    }

    


    // Start is called before the first frame update
    void Start()
    {
        harpoonGun.reel.Play();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            default:
            case State.InAir:
                InAirAI();
                break;
            case State.HitEnemy:
                HitEnemyAI();
                break;
            case State.HitItem:
                HitItemAI();
                break;
            case State.HitGrapple:
                HitGrappleAI();
                break;
            case State.ReelIn:
                ReelInAI();
                break;
        }        
    }

    private void InAirAI()
    {
        if (Vector3.Distance(initialPlayerPosition, this.transform.position) >= harpoonRange)
        {
            SetLerpProperties();
            endPoint = harpoonGun.harpoonSpawnPoint;
            startPoint = this.transform.position;
            state = State.ReelIn;
        }
    }

    private void HitEnemyAI()
    {
        
        reelTimer += Time.deltaTime;
        float percentageComplete = reelTimer / maxReelTime;
        this.transform.position = Vector3.Lerp(startPoint, endPoint.position, percentageComplete);
        hitEnemy.transform.localScale = Vector3.Lerp(startScale, endScale, percentageComplete);
        hitEnemy.transform.position = this.transform.position;
        if (this.transform.position == endPoint.position)
        {
            //NOTE: Consider making a lerp for the enemy as well to make them scale down in size while flying at the player so 
            //the player's FOV is not obstructed

            //NOTE: Add a function for Fish-O-Pedia logging
            harpoonGun.reel.Stop();
            harpoonGun.clickIn.Play();
            harpoonGun.SendBulletToGun(hitEnemy.bulletType);
            harpoonGun.ResetFire();
            Destroy(hitEnemy.gameObject);
            hitEnemy = null;
            Destroy(this.gameObject);
        }
    }

    private void HitItemAI()
    {
        
    }

    private void HitGrappleAI()
    {
        harpoonGun.playerRB.velocity = grappleTrajectory * grappleSpeed * 10f;
        if(Vector3.Distance(grapplePoint.transform.position, harpoonGun.transform.position) < 1)
        {
            harpoonGun.reel.Stop();
            harpoonGun.clickIn.Play();
            harpoonGun.playerRB.velocity = new Vector3(harpoonGun.playerRB.velocity.x, 15, harpoonGun.playerRB.velocity.z);
            harpoonGun.ResetFire();
            Destroy(this.gameObject);
        }
    }

    private void ReelInAI()
    {
        rb.velocity = Vector3.zero;
        
        reelTimer += Time.deltaTime;
        float percentageComplete = reelTimer / maxReelTime;
        Debug.Log(percentageComplete);
        this.transform.position = Vector3.Lerp(startPoint, endPoint.position, percentageComplete);
        if (this.transform.position == endPoint.position)
        {
            harpoonGun.reel.Stop();
            harpoonGun.clickIn.Play();
            harpoonGun.ResetFire();
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state == State.InAir)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                SetLerpProperties();

                rb.velocity = Vector3.zero;
                Destroy(rb);
                hitEnemy = collision.gameObject.GetComponent<EnemyHealthAndQTE>();
                if (hitEnemy.harpoonable)
                {
                    //hitEnemy.gameObject.transform.parent = this.transform;
                    //hitEnemy.boxCollider.enabled = false;
                    startPoint = this.transform.position;
                    
                    state = State.HitEnemy;
                }
                else
                {
                    startPoint = this.transform.position;
                    
                    state = State.ReelIn;
                }
            }

            if (collision.gameObject.CompareTag("GrapplePoint"))
            {
                SetLerpProperties();

                rb.velocity = Vector3.zero;
                Destroy(rb);
                grapplePoint = collision.gameObject;
                grappleTrajectory = grapplePoint.transform.position - harpoonGun.transform.position;
                state = State.HitGrapple;
            }

            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
            {
                SetLerpProperties();
                Debug.Log("Failed Hit on: " + collision.gameObject.tag + ". Harpoon will now be reeled in.");
                startPoint = this.transform.position;
                state = State.ReelIn;
            }
            if (collision.gameObject.CompareTag("Player"))
            {
                
            }
        }
    }

    private void SetLerpProperties()
    {

        maxReelTime = Vector3.Distance(harpoonGun.harpoonSpawnPoint.position, this.transform.position) / harpoonRange;
        if(maxReelTime > 1)
        {
            maxReelTime = 1;
        }
        maxReelTime /= 4;
        Debug.Log("Max Reel Time: " + maxReelTime);
        endPoint = harpoonGun.harpoonSpawnPoint;
    }
}
