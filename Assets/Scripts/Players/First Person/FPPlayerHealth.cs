using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPlayerHealth : MonoBehaviour
{
    [Header("Health Tracking")]
    public int currentHealth;
    [SerializeField] private int maxHealth;

    public bool isDead;

    [Header("Debug Tools")]
    public bool dieBelowYOrigin;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (dieBelowYOrigin)
        {
            DieBelowYOrigin();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            isDead = true;

            //NOTE: Update this to throw to a Game Over Screen when one
            //is added to the game.
            GameManager.LoadScene("FPS Level");
        }
    }

    public void DieBelowYOrigin()
    {
        if(transform.position.y <= 0)
        {
            GameManager.LoadScene("FPS Level");
        }
    }
}
