using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] HealthManager healthManager = null;
    [SerializeField] RectTransform forground = null;
    [SerializeField] Canvas rootCanvas = null;

    [Tooltip("Hide the health bar if entity at full health or dead.")]
    public bool hideHealhbar = false;


    private void Update()
    {

        if ((Mathf.Approximately(healthManager.GetFraction(), 0)
            || Mathf.Approximately(healthManager.GetFraction(), 1)) && hideHealhbar)
        {
            rootCanvas.enabled = false;
            return;
        }

        rootCanvas.enabled = true;
        forground.localScale = new Vector3(healthManager.GetFraction(), 1, 1);
    }
}    