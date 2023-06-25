using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthAndQTE : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject boxPattern;
    public List<QTEActivationBox> allBoxes;
    public BoxCollider boxCollider;
    public bool harpoonable;
    public bool isDead;
    public string bulletType;

    public int currentHealth;
    public int harpoonHealthRange;
    [SerializeField] private int maxHealth;

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
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }
}
