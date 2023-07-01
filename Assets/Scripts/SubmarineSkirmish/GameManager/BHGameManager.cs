using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHGameManager : MonoBehaviour
{ 
    public int saf;
    public BHPlayerController playerController;
    public BHEnemyManager enemyManager; 

    private int radius;
    public int Radius
    {
        get { return radius; }
        set
        {
            if (radius != value)
            {
                radius = value;
                UpdateRadius();
            }
        }
    }

    private void Update()
    {
        Radius = saf;
    }

    private void UpdateRadius()
    {
        playerController.m_Radius = radius;
        //playerController.UpdateRadius(radius);
    }
}
