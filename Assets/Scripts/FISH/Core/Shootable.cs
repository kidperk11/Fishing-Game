using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public bool shot;
    public int shotCounter;

    public void Activate()
    {
        shot = true;
        shotCounter++;
    }

    public void Deactivate()
    {
        shot = false;
        shotCounter++;
    }
}
