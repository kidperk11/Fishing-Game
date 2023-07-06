using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDeathBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            //NOTE: Change this code later to load a game over screen.

            GameManager.LoadScene("FPS Level");
        }
    }
}
