using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPlayerHealth : MonoBehaviour
{
    public int currentHealth;
    [SerializeField] private int maxHealth;

    public bool isDead;

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

            //NOTE: Update this to throw to a Game Over Screen when one
            //is added to the game.
            GameManager.LoadScene("FPS Level");
        }
    }
}
