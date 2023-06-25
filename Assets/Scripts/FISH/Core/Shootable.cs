using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public bool shot;

    public void Activate()
    {
        shot = true;
    }
}
