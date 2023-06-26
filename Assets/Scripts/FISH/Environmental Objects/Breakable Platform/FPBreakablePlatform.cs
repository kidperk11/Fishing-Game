using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPBreakablePlatform : MonoBehaviour
{
    [Header("External References")]
    public Shootable shootable;

    [Header("Health")]
    public int platformHealth;

    private void Update()
    {
        if(shootable.shotCounter >= platformHealth)
        {
            Destroy(this.gameObject);
        }
    }
}
