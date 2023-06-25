using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormGame : MonoBehaviour
{
    private MiniGameManager miniGameManager;
    [SerializeField] private ValueHolder wormsRemaining;

    [Header("Sound Effects")]
    public AudioSource audioPlayer;
    public AudioClip successSound;
    public AudioClip failureSound;
    public AudioClip beatMiniGameSound;

    private void Start()
    {
        miniGameManager = GameObject.FindGameObjectWithTag("Austin-3").GetComponent<MiniGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(wormsRemaining.integerValue == 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        audioPlayer.clip = beatMiniGameSound;
        audioPlayer.Play();
        //Add code to sequence to the next mini game;
        miniGameManager.StartChangeGame();
    }

    public void WormCaptured()
    {

    }
}
