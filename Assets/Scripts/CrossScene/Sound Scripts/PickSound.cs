using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickSound : MonoBehaviour
{

    public AudioSource[] audioSources;
    void SelectSource(int index){
        audioSources[index].Play();
    }

    void StopSource(int index){
        audioSources[index].Stop();
    }
}
