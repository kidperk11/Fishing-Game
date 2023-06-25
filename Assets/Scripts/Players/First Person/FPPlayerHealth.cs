using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPPlayerHealth : MonoBehaviour
{
    [Header("Health Tracking")]
    public int currentHealth;
    [SerializeField] private int maxHealth;
    public bool isDead;
    public bool takingKnockback;
    public float knockbackTimer;
    [SerializeField] private float maxKnockbackTime;
    

    [Header("UI Elements")]
    public TextMeshProUGUI healthText;

    [Header("External References")]
    public Rigidbody rb;

    [Header("Debug Tools")]
    public bool dieBelowYOrigin;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthText.text = new string("Health: " + currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (dieBelowYOrigin)
        {
            DieBelowYOrigin();
        }

        if (takingKnockback)
        {
            knockbackTimer += Time.deltaTime;
            if(knockbackTimer >= maxKnockbackTime)
            {
                knockbackTimer = 0;
                takingKnockback = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            isDead = true;

            //NOTE: Update this to throw to a Game Over Screen when one
            //is added to the game.
            GameManager.LoadScene("FPS Level");
        }
        else
        {
            healthText.text = new string("Health: " + currentHealth);
        }
    }
    public void TakeKnockback(Vector3 knockbackDirection, float knockbackForce)
    {
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        takingKnockback = true;
    }

    public void DieBelowYOrigin()
    {
        if(transform.position.y <= 0)
        {
            GameManager.LoadScene("FPS Level");
        }
    }
}
