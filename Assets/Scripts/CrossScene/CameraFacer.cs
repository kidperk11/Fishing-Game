using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFacer : MonoBehaviour
{
    public bool ignoreZaxis;
    public bool disableOnSpawn;

    private void Start()
    {
        if (disableOnSpawn)
        {
            transform.forward = Camera.main.transform.forward;
            this.enabled = false;
        }

    }

    private void LateUpdate()
    {
            transform.forward = Camera.main.transform.forward;
    }
}
