using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPiston : MonoBehaviour
{
    [Header("External References")]
    public Shootable shootableSwitch;
    public Transform startTransform;
    public Transform endTransform;

    [Header("Movement and Lerp Variables")]
    [SerializeField] private float maxFireTime;
    [SerializeField] private float maxRetractTime;
    private float fireTimer;
    private float retractTimer;
    [SerializeField] private float enemyKnockbackSpeed;
    [SerializeField] private float playerKnockbackSpeed;
    [SerializeField] private float playerYKnockbackDirection;

    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Quaternion endRotation;
    private float lerpTimer;
    private List<EnemyHealth> hitEnemies = new List<EnemyHealth>();
    public bool hitPlayer;

    [Header("Debug Tools")]
    public bool hasHarpoonSwitch;

    public State state;
    public enum State
    {
        idle,
        firing,
        extended,
        retracting
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.idle:
                IdleAI();
                break;
            case State.firing:
                FiringAI();
                break;
            case State.extended:
                ExtendedAI();
                break;
            case State.retracting:
                RetractingAI();
                break;
        }
    }

    private void IdleAI()
    {
        if (shootableSwitch.shot)
        {
            //NOTE: Consider adding logic to change the color of the harpoonSwitch when it's active
            state = State.firing;
        }
    }

    private void FiringAI()
    {
        lerpTimer += Time.deltaTime;
        float percentageComplete = lerpTimer / maxFireTime;
        this.transform.position = Vector3.Lerp(startTransform.position, endTransform.position, percentageComplete);
        //this.transform.rotation = Quaternion.Lerp(startRotation, endRotation, percentageComplete);
        if(percentageComplete >= 1)
        {
            lerpTimer = 0;
            hitPlayer = false;
            state = State.extended;
        }
    }

    private void ExtendedAI()
    {
        if (!shootableSwitch.shot)
        {
            state = State.retracting;
        }
    }

    private void RetractingAI()
    {
        lerpTimer += Time.deltaTime;
        float percentageComplete = lerpTimer / maxRetractTime;
        this.transform.position = Vector3.Lerp(endTransform.position, startTransform.position, percentageComplete);
        //this.transform.rotation = Quaternion.Lerp(endRotation, startRotation, percentageComplete);
        if (percentageComplete >= 1)
        {
            lerpTimer = 0;
            state = State.idle;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(state == State.firing)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
                //Vector3 contactDirection = collision.gameObject.transform.position - this.transform.position;
                Vector3 contactDirection = transform.forward;
                enemy.TakeKnockback(contactDirection, enemyKnockbackSpeed);
                Debug.Log("Enemy hit by piston");
            }
            if (collision.gameObject.CompareTag("Player") && !hitPlayer)
            {
                hitPlayer = true;
                FPPlayerHealth player = collision.gameObject.GetComponent<FPPlayerHealth>();
                //Vector3 contactDirection = collision.gameObject.transform.position - this.transform.position;
                Vector3 contactDirection = transform.forward;
                player.TakeKnockback(new Vector3(contactDirection.x, playerYKnockbackDirection, contactDirection.z), playerKnockbackSpeed);
            }
        }
    }



}
