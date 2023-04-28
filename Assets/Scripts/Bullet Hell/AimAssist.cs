using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    public Transform player;
    public Transform aimTarget;
    public Transform playerCamera;

    public float maxAngle = 10.0f;
    public float aimAssistMultiplier = 5.0f;
    public float lerpMultiplier = 0.5f;

    private Vector3 lerpSmoothing;

    void Update()
    {
        Vector3 cameraDirection = (aimTarget.position - playerCamera.position).normalized;

        float angle = Vector3.Angle(player.forward, cameraDirection);
        if (angle < maxAngle)
        {
            Vector3 targetAimDirection = Vector3.RotateTowards(player.forward, cameraDirection, Time.deltaTime * aimAssistMultiplier, 0.0f);

            lerpSmoothing = Vector3.Lerp(lerpSmoothing, targetAimDirection, Time.deltaTime * lerpMultiplier);

            player.rotation = Quaternion.LookRotation(lerpSmoothing);
        }
    }
}
