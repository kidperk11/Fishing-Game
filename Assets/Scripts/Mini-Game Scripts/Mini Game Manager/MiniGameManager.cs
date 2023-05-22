using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameManager : MonoBehaviour
{
    public List<GameObject> miniGames;
    [TextArea(1,5)]
    public List<string> transitionTextList;
    private int index;

    public Animator anim;

    public TextMeshProUGUI transitionText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitialTransition());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartChangeGame()
    {
        StartCoroutine(StartTransition());
    }
    
    public IEnumerator StartTransition()
    {
        //Wait for the little effects to wear off
        yield return new WaitForSecondsRealtime(2);
        
        //Begin the "Transition" animation;
        anim.SetTrigger("transition");
        
        //Wait for the transition to fade all the way to black
        yield return new WaitForSecondsRealtime(1);

        //Pause time
        Time.timeScale = 0;

        //Set the indexer, turn off the old mini game, and turn on the new one
        miniGames[index].SetActive(false);
        index += 1;
        miniGames[index].SetActive(true);
        transitionText.text = transitionTextList[index];
        
    }

    //This function should be called in the animation as an event;
    public void EndTransition()
    {
        //Set timescale back to normal
        Time.timeScale = 1;
    }

    public IEnumerator InitialTransition()
    {
        //Wait for the little effects to wear off
        yield return new WaitForSecondsRealtime(2);

        //Begin the "Transition" animation;
        anim.SetTrigger("transition");

        //Wait for the transition to fade all the way to black
        yield return new WaitForSecondsRealtime(1);

        //Pause time
        Time.timeScale = 0;

        miniGames[index].SetActive(true);
        transitionText.text = transitionTextList[index];
    }
}
