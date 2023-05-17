using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;


public class SandwitchPiece : MonoBehaviour
{
    [Header("Movement")]
    public Transform rightHoverPoint;
    private Rigidbody2D rb;
    public float cycleTime;

    [Header("Sequencing")]
    public GameObject nextPiece;

    [Header("Sound Effects")]
    public AudioSource successSound;
    public AudioSource failureSound;

    [Header("Input Actions")]
    public FPPlayerActions moveActions;
    private InputAction actionButton;
    private bool actionReady;

    private void OnEnable()
    {   
        actionButton = moveActions.Player.Jump;
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
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
        rb.freezeRotation = true;
        transform.DOMove(rightHoverPoint.position, cycleTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ActionInput(InputAction.CallbackContext context)
    {

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void ResetActionInput()
    {
        
    }
}
