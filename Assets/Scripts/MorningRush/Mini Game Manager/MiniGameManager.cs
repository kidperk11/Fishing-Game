using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameManager : MonoBehaviour
{
    private int index;

    public Animator anim;

    public TextMeshProUGUI transitionText;

    public SpriteRenderer minigameBackground;

    public List<MiniGameScriptableObject> miniGame;

    private GameObject currentGame;
    private GameObject nextGame;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;

        currentGame = Instantiate(miniGame[index].miniGamePrefab, Vector3.zero, Quaternion.identity);

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
        index += 1;

        //Prepare the next minigame
        currentGame.SetActive(false);
        nextGame.SetActive(true);
        Destroy(currentGame);
        currentGame = nextGame;

        //Load in the next game in background to prevent visible loading
        nextGame = Instantiate(miniGame[index + 1].miniGamePrefab, Vector3.zero, Quaternion.identity);
        minigameBackground.sprite = miniGame[index].background;
        nextGame.SetActive(false);

        transitionText.text = miniGame[index].transitionText;
        
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

        //Load in the next game in background to prevent visible loading
        nextGame = Instantiate(miniGame[index + 1].miniGamePrefab, Vector3.zero, Quaternion.identity);
        nextGame.SetActive(false);

        //Begin the "Transition" animation;
        anim.SetTrigger("transition");

        //Wait for the transition to fade all the way to black
        yield return new WaitForSecondsRealtime(1);

        //Pause time
        Time.timeScale = 0;
        minigameBackground.sprite = miniGame[index].background;

        currentGame.SetActive(true);
        transitionText.text = miniGame[index].transitionText;
    }
}
