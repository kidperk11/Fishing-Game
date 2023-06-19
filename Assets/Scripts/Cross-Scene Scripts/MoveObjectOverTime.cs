using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectOverTime : MonoBehaviour
{
    //public Transform startPos;  // The starting position of the GameObject
    public Vector3 startPos;
    //public Transform endPos;    // The target position of the GameObject
    public Vector3 endPos;    // The target position of the GameObject
    public float duration = 2f; // The duration in seconds for the movement

    public Vector3 startScale;
    public Vector3 endScale;

    public float delayDuration = 0f;

    void Start()
    {
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(delayDuration);
        StartCoroutine(MoveOverTime());
    }

    IEnumerator MoveOverTime()
    {
        float elapsedTime = 0f; // The elapsed time since the movement started

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the normalized progress (0 to 1) based on the elapsed time and duration
            float normalizedProgress = Mathf.Clamp01(elapsedTime / duration);

            // Interpolate between the start and end positions using the normalized progress
            transform.localPosition = Vector3.Lerp(startPos, endPos, normalizedProgress);
            transform.localScale = Vector3.Lerp(startScale, endScale, normalizedProgress);

            yield return null;
        }

        // Movement complete, do any necessary actions
    }
}
