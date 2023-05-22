using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;


public class SandwichPiece : MonoBehaviour
{
    public MiniGameManager miniGameManager;
    [Header("Movement")]
    private Tween tween;
    public Transform leftHoverPoint;
    public Transform rightHoverPoint;
    private Rigidbody2D rb;
    public float cycleTime;

    [Header("Sequencing")]
    public GameObject nextPiece;

    [Header("Sound Effects")]
    public AudioSource successSound;
    public AudioSource failureSound;
    public AudioSource beatMiniGameSound;

    [Header("Input Actions")]
    public FPPlayerActions moveActions;
    private InputAction actionButton;
    private bool actionReady;

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
        actionReady = true;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
        rb.freezeRotation = true;
        tween = transform.DOMove(rightHoverPoint.position, cycleTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ActionInput(InputAction.CallbackContext context)
    {
        if (actionReady)
        {
            actionReady = false;
            tween.Kill();
            rb.velocity = Vector2.zero;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Meat"))
        {
            
            Destroy(rb);
            if(nextPiece != null)
            {
                successSound.Play();
                nextPiece.SetActive(true);
                Destroy(this);
            }
            else
            {
                beatMiniGameSound.Play();
                //Add code to sequence to the next mini game;
                miniGameManager.StartChangeGame();
            }
        }
        else if (collision.gameObject.CompareTag("Table"))
        {
            failureSound.Play();
            this.transform.position = leftHoverPoint.position;
            tween = transform.DOMove(rightHoverPoint.position, cycleTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            ResetActionInput();
        }
    }

    private void ResetActionInput()
    {
        actionReady = true;
    }
}
