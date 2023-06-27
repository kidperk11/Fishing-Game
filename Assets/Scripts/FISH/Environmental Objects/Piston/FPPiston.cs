using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPiston : MonoBehaviour
{
    [Header("External References")]
    public Rigidbody rb;
    public HarpoonSwitch harpoonSwitch;
    public Transform startTransform;
    public Transform endTransform;

    [Header("Movement Variables")]
    [SerializeField] private float maxFireTime;
    [SerializeField] private float maxRetractTime;
    private float fireTimer;
    private float retractTimer;
    [SerializeField] private float knockbackSpeed;

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
        
    }

    private void FiringAI()
    {
        
    }

    private void ExtendedAI()
    {
        
    }

    private void RetractingAI()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state == State.firing)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyHealthAndQTE enemy = collision.gameObject.GetComponent<EnemyHealthAndQTE>();
                Vector3 contactDirection = collision.gameObject.transform.position - this.transform.position;
                enemy.TakeKnockback(contactDirection, knockbackSpeed);
            }
        }
    }



}
