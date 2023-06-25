using UnityEngine;
using UnityEngine.Events;

public class BHHealthManager : MonoBehaviour
{
    public int maxHitPoints;

    [Tooltip("Time that this gameObject is invulnerable for, after receiving damage.")]
    public float invulnerabiltyTime;

    public bool isInvulnerable { get; set; }
    public float currentHitPoints { get; set; }

    public bool isDead;

    public UnityEvent OnDeath, OnReceiveDamage, OnHitWhileInvulnerable, OnBecomeVulnerable, OnResetDamage;

    protected float timeSinceLastHit = 0.0f;
    protected Collider hitCollider;


    private void Start()
    {
        ResetDamage();
        hitCollider = GetComponent<Collider>();
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.H))
        {
            ApplyDamage(1f);
        }

        if (isInvulnerable)
        {
            timeSinceLastHit += Time.deltaTime;
            if (timeSinceLastHit > invulnerabiltyTime)
            {
                timeSinceLastHit = 0.0f;
                isInvulnerable = false;
                OnBecomeVulnerable.Invoke();
            }
        }
    }

    public void ResetDamage()
    {
        currentHitPoints = maxHitPoints;
        isInvulnerable = false;
        timeSinceLastHit = 0.0f;
        OnResetDamage.Invoke();
    }

    public void SetColliderState(bool enabled)
    {
        hitCollider.enabled = enabled;
    }

    public float GetFraction()
    {
        return currentHitPoints / maxHitPoints;
    }

    public void ApplyDamage(float damageAmount)
    {
        currentHitPoints -= damageAmount;
        if (currentHitPoints <= 0 && !isDead)
        {
            Die();
        }
        else
        {
            OnReceiveDamage.Invoke();
        }
    }

    public void Die()
    {
        isDead = true;
        //GetComponentInChildren<Animator>().SetTrigger("Death");
        OnDeath.Invoke();
    }
}