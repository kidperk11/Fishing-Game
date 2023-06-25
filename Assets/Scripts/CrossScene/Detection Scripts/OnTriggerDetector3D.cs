using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerDetector3D : MonoBehaviour
{
    [Header("List of Detectable Tags")]
    public List<string> tags = new List<string>();
    public List<string> detectedTags = new List<string>();

    private void OnTriggerEnter(Collider other)
    {
        foreach(string tag in tags)
        {
            if (other.CompareTag(tag))
            {
                detectedTags.Add(tag);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (string tag in tags)
        {
            if (other.CompareTag(tag))
            {
                detectedTags.Remove(tag);
                Debug.Log("Target of " + tag + " no longer detected by " + transform.parent.gameObject);
            }
        }
    }

    public bool CheckIfTagDetected(string tagToCheck)
    {
        foreach(string tag in detectedTags)
        {
            if(tag == tagToCheck)
            {
                return true;
            }
        }
        return false;
    }



}
