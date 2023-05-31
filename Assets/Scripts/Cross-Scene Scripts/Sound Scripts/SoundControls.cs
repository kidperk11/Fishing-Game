using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControls : MonoBehaviour
{
    public AudioSource audioSource;
    public void Play(){
        
        audioSource.Play();
    }

    public void Stop(){
        audioSource.Stop();
    }
}
