using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FPSUrchinAI : MonoBehaviour
{
    [Header("External References")]
    public Rigidbody rb;
    public NavAgentCollisionManagement collisionManagement;
    public NavMeshAgent agent;
    public EnemyHealth health;
    public Animator anim;
    public FPPlayerHealth player;
    public OnTriggerDetector3D detector;
    public SphereCollider weakPointCollider;

    [Header("Movement Properties")]
    public float normalSpeed;
    public float attackSpeed;

    [Header("Aim Properties")]
    public float maxAimTime;
    private float aimTimer;

    [Header("Attack Properties")]
    public float playerDetectionDistance;
    public float playerKnockbackSpeed;
    public float playerYKnockbackDirection;
    public float attackDistance;
    private Vector3 attackStartingPoint;
    [SerializeField] private int attackDamage;

    [Header("Cooldown Properties")]
    public float maxCooldownTime;
    private float cooldownTimer;

    [Header("Harpoonable Properties")]
    public GameObject weakPoint;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health.isDead)
        {
            //NOTE: Add code for the death animation
            state = State.Death;
        }
        if (collisionManagement.isRagdoll)
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
        if(aimTimer >= maxAimTime)
        {
            aimTimer = 0;
            anim.SetTrigger("attack");
            attackStartingPoint = this.transform.position;
            agent.speed = attackSpeed;
            agent.ResetPath();
            //agent.SetDestination(transform.position + transform.forward * attackDistance);
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
        if (detector.CheckIfTagDetected("Player"))
        {
            player.TakeDamage(attackDamage);
            Vector3 contactDirection = player.gameObject.transform.position - this.transform.position;
            player.TakeKnockback(new Vector3(contactDirection.x, playerYKnockbackDirection, contactDirection.z), playerKnockbackSpeed);
            agent.speed = normalSpeed;
            agent.SetDestination(this.transform.position);
            //NOTE: Add code for idle animation
            state = State.Cooldown;
        }
        else if(detector.CheckIfTagDetected("Wall") || detector.CheckIfTagDetected("Ground"))
        {
            anim.SetTrigger("harpoonable");
            weakPointCollider.enabled = true;
            weakPoint.SetActive(true);
            health.enabled = true;
            agent.speed = normalSpeed;
            agent.SetDestination(this.transform.position);
            agent.velocity = Vector3.zero;
            agent.enabled = false;
            rb.isKinematic = false;
            state = State.Harpoonable;
        }
        else if(Vector3.Distance(this.transform.position, attackStartingPoint) >= (attackDistance-1))
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
        if(cooldownTimer >= maxCooldownTime)
        {
            cooldownTimer = 0;
            anim.SetTrigger("chase");
            state = State.ChasePlayer;
        }
    }

    private void HarpoonableAI()
    {
        harpoonableTimer += Time.deltaTime;
        if(harpoonableTimer >= maxHarpoonableTime)
        {
            weakPointCollider.enabled = false;
            weakPoint.SetActive(false);
            health.enabled = false;
            harpoonableTimer = 0;
            anim.SetTrigger("chase");
            weakPoint.SetActive(false);
            rb.isKinematic = true;
            agent.enabled = true;
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
        throw new NotImplementedException();
    }

    
}
