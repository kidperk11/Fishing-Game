using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [Header("External References")]
    public Rigidbody rb;
    public BoxCollider boxCollider;
    public NavAgentCollisionManagement agentCollisionManagement;
    
    [Header("Health Tracking Variables")]
    public int currentHealth;
    public int harpoonHealthRange;
    [SerializeField] private int maxHealth;
    public bool harpoonable;
    public bool isDead;
    public string bulletType;

    [Header("NavAgent Specific Variables")]
    public NavMeshAgent agent;
    [SerializeField] private float maxNavKnockbackTime;
    private Vector3 navKnockbackVelocity;
    private float navKnockbackTimer;
    private bool takingNavKnockback;

    [Header("Behavior Toggles")]
    public bool isNavAgent;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            isDead = true;
        } else if(currentHealth <= harpoonHealthRange)
        {
            harpoonable = true;
        }
    }

    public void TakeKnockback(Vector3 knockbackDirection, float knockbackForce)
    {
        if (isNavAgent)
        {
            Debug.Log(this.gameObject);
            agentCollisionManagement.TakeKnockBack(knockbackDirection, knockbackForce);
        }
        else
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
        
    }
}
