using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FPPiranhaAI : MonoBehaviour
{
    [Header("External References")]
    public NavMeshAgent agent;
    public EnemyHealth health;
    public Animator anim;
    public FPPlayerHealth player;
    public OnTriggerDetector3D detector;
    public NavAgentCollisionManagement collisionManagement;

    [Header("Movement Properties")]
    public float normalSpeed;
    public float attackSpeed;

    [Header("Aim Properties")]
    public float maxAimTime;
    private float aimTimer;

    [Header("Attack Properties")]
    public float playerDetectionDistance;
    //public float playerKnockbackSpeed;
    //public float playerYKnockbackDirection;
    public float attackDistance;
    private Vector3 attackStartingPoint;
    [SerializeField] private int attackDamage;

    [Header("Cooldown Properties")]
    public float maxCooldownTime;
    private float cooldownTimer;

    [Header("Harpoonable Properties")]
    public float maxHarpoonableTime;
    private float harpoonableTimer;

    [Header("Debug Tools")]
    [SerializeField] private bool skipSpawn;

    public State state;
    public enum State
    {
        SpawnIn,
        ChasePlayer,
        AimAtPlayer,
        Attack,
        Cooldown,
        Harpoonable,
        Ragdoll,
        Death
    }

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = normalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (health.harpoonable && state != State.Ragdoll)
        {
            agent.SetDestination(this.transform.position);
            agent.velocity = Vector3.zero;
            state = State.Harpoonable;
        }
        if (health.isDead)
        {
            //NOTE: Add code for the death animation
            state = State.Death;
        }
        if (collisionManagement.isRagdoll && !health.isDead)
        {
            state = State.Ragdoll;
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
            case State.AimAtPlayer:
                AimAtPlayerAI();
                break;
            case State.Attack:
                AttackAI();
                break;
            case State.Cooldown:
                CooldownAI();
                break;
            case State.Harpoonable:
                HarpoonableAI();
                break;
            case State.Ragdoll:
                RagdollAI();
                break;
            case State.Death:
                DeathAI();
                break;
        }
    }

    

    private void SpawnInAI()
    {
        if (skipSpawn)
        {
            state = State.ChasePlayer;
        }

        //NOTE: Add code later for a spawner Animation;
    }

    private void ChasePlayerAI()
    {
        agent.SetDestination(player.transform.position);
        transform.LookAt(transform.forward);
        if (Vector3.Distance(this.transform.position, player.transform.position) <= playerDetectionDistance)
        {
            agent.SetDestination(this.transform.position);
            anim.SetTrigger("aimAtPlayer");
            state = State.AimAtPlayer;
        }
    }

    private void AimAtPlayerAI()
    {
        aimTimer += Time.deltaTime;
        if (aimTimer >= maxAimTime)
        {
            aimTimer = 0;
            anim.SetTrigger("attack");
            attackStartingPoint = this.transform.position;
            agent.speed = attackSpeed;
            agent.ResetPath();
            state = State.Attack;
        }
        else
        {
            transform.LookAt(player.transform);
        }
    }

    private void AttackAI()
    {
        agent.velocity = transform.forward * attackSpeed;
        if (Vector3.Distance(this.transform.position, attackStartingPoint) >= (attackDistance - 1))
        {
            anim.SetTrigger("chase");
            agent.speed = normalSpeed;
            agent.SetDestination(this.transform.position);
            agent.velocity = Vector3.zero;
            state = State.Cooldown;
        }
    }

    private void CooldownAI()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= maxCooldownTime)
        {
            cooldownTimer = 0;
            anim.SetTrigger("chase");
            state = State.ChasePlayer;
        }
    }

    private void HarpoonableAI()
    {
        harpoonableTimer += Time.deltaTime;
        if (harpoonableTimer >= maxHarpoonableTime)
        {
            health.harpoonable = false;
            harpoonableTimer = 0;
            anim.SetTrigger("chase");
            state = State.ChasePlayer;
        }
    }

    private void RagdollAI()
    {
        if (!collisionManagement.isRagdoll)
        {
            state = State.ChasePlayer;
        }
    }

    private void DeathAI()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && state == State.Attack)
        {
            player.TakeDamage(attackDamage);
            agent.velocity = Vector3.zero;
            state = State.Cooldown;
        }
    }
}
