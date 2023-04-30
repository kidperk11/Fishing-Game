using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPufferAI : MonoBehaviour
{
    //Basic Variables
    public EnemyHealthAndQTE health;
    public Animator anim;
    public FPPlayerHealth playerHealth;

    //SpawnIn
    [SerializeField] private bool skipSpawn;

    //ChasePlayer
    [SerializeField] private float maxChaseTime;
    [SerializeField] private float currentChaseTimer;
    [SerializeField] private float moveSpeed;


    //FireBullet
    public GameObject bulletPrefab;
    public Transform bulletLaunchTransform;



    public State state;

    public enum State
    {
        SpawnIn,
        ChasePlayer,
        FireBullet,
        Hurt,
        Harpoonable,
        Death
    }
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindAnyObjectByType<FPPlayerHealth>().GetComponent<FPPlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.harpoonable && state != State.Harpoonable)
        {
            //NOTE: Add code for the harpoonable animation
            anim.SetTrigger("hurt");
            state = State.Harpoonable;
        }

        if (health.isDead)
        {
            //NOTE: Add code for the death animation
            state = State.Death;
        }

        switch (state)
        {
            default:
            case State.SpawnIn:
                SpawnInAI();
                break;
            case State.ChasePlayer:
                ChasePlayerAI();
                break;
            case State.FireBullet:
                FireBulletAI();
                break;
            case State.Hurt:
                HurtAI();
                break;
            case State.Harpoonable:
                HarpoonableAI();
                break;
            case State.Death:
                DeathAI();
                break;
        }
    }

    private void ChasePlayerAI()
    {
        currentChaseTimer -= Time.deltaTime;
        if(currentChaseTimer <= 0)
        {
            currentChaseTimer = maxChaseTime;
            anim.SetTrigger("fireBullet");
            state = State.FireBullet;
        }
        transform.LookAt(playerHealth.transform);
    }

    private void SpawnInAI()
    {
        if (skipSpawn)
        {
            currentChaseTimer = maxChaseTime;
            state = State.ChasePlayer;
        }

        //NOTE: Add code later for a spawner Animation;
    }

    private void FireBulletAI()
    {
        transform.LookAt(playerHealth.transform);
    }

    private void HurtAI()
    {
        
    }

    private void HarpoonableAI()
    {
        transform.LookAt(playerHealth.transform);
    }

    private void DeathAI()
    {
        //NOTE: Add code for death timer, disable hitboxes, etc
        Destroy(this.gameObject);
    }

    public void InstantiateBullet()
    {
        FPPufferBulletAI bullet = Instantiate(bulletPrefab, bulletLaunchTransform.position, Quaternion.identity).GetComponent<FPPufferBulletAI>();
        Vector3 directionWithoutSpread = playerHealth.transform.position - bulletLaunchTransform.position;
        bullet.transform.forward = directionWithoutSpread.normalized;
        bullet.rb.AddForce(transform.forward * bullet.bulletSpeed, ForceMode.Impulse);
        bullet.player = playerHealth.gameObject;
        //bullet.rb.AddForce(Vector3.forward * bullet.bulletSpeed, )
    }

    public void EndHarpoonable()
    {
        health.harpoonable = false;
        anim.SetTrigger("fireBullet");
        state = State.FireBullet;
    }

    public void EndFireBullet()
    {
        state = State.ChasePlayer;
    }
}
