using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class BoatEngineNeedleTicker : MonoBehaviour
{
    public MiniGameManager miniGameManager;

    [Header("Movement")]
    private Tween tween;
    public float cycleTime;
    public Rigidbody2D rb;

    [Header("Object Management")]
    public int numberOfWindows;
    public int completedWindows;

    [Header("Input Actions")]
    public FPPlayerActions moveActions;
    private InputAction actionButton;
    private bool windowActive;
    private GameObject currentWindow;

    [Header("Sound Effects")]
    public AudioSource successSound;
    public AudioSource failureSound;
    public AudioSource beatMiniGameSound;

    private void OnEnable()
    {
        actionButton = moveActions.Player.Gun;
        actionButton.Enable();
        actionButton.performed += ActionInput;
    }

    private void OnDisable()
    {
        actionButton.Disable();
    }

    private void Awake()
    {
        moveActions = new FPPlayerActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        miniGameManager = GameObject.FindGameObjectWithTag("Austin-3").GetComponent<MiniGameManager>();
        tween = transform.DORotate(new Vector3(0, 0, 360), cycleTime, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear).SetRelative();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("QTESuccessWindow"))
        {
            windowActive = true;
            currentWindow = collision.gameObject;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("QTESuccessWindow"))
        {
            windowActive = false;
            currentWindow = null;
        }
    }

    private void ActionInput(InputAction.CallbackContext context)
    {
        if (windowActive)
        {
            Success();
        }
        else
        {
            failureSound.Play();
        }
        
            //tween.Kill();
            //rb.velocity = Vector2.zero;
        
    }
  
    private void Success()
    {
        
        currentWindow.gameObject.SetActive(false);
        completedWindows += 1;
        if (numberOfWindows == completedWindows)
        {
            beatMiniGameSound.Play();
            tween.Kill();
            miniGameManager.StartChangeGame();
            //Add code to sequence to the next mini game;
        }
        else
        {
            successSound.Play();
        }
    }
}
