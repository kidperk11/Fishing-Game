using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSTargetWall : MonoBehaviour
{
    [Header("External References")]
    public Shootable target;


    [Header("Lerp Variables")]
    public Transform endTransform;
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Quaternion endRotation;
    [SerializeField] private float lerpTimer;
    [SerializeField] private float maxLerpTime;



    private bool movementComplete;

    private void Start()
    {
        startPoint = this.transform.position;
        startRotation = this.transform.rotation;
        endPoint = endTransform.position;
        endRotation = endTransform.rotation;
    }


    private void Update()
    {
        if (target.shot)
        {
            lerpTimer += Time.deltaTime;
            float percentageComplete = lerpTimer / maxLerpTime;
            this.transform.position = Vector3.Lerp(startPoint, endPoint, percentageComplete);
            this.transform.rotation = Quaternion.Lerp(startRotation, endRotation, percentageComplete);            
        }
    }
}
