using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class AlarmClockHandController : MonoBehaviour
{
    public MiniGameManager miniGameManager;
    [Header("Movement")]
    private Tween tween;
    public Transform leftHoverPoint;
    public Transform rightHoverPoint;
    public float cycleTime;
    public bool inClockRange;
    public Transform successStartPoint;
    public Transform successEndPoint;
    public Transform missHeight;

    [Header("Input Actions")]
    public FPPlayerActions moveActions;
    private InputAction actionButton;
    private bool actionReady;

    [Header("Miss State")]
    public float maxMissTime;
    private float missTimer;

    [Header("Success State")]
    public float maxSuccessTime;
    private float successTimer;

    [Header("State Machine")]
    public State state;
    public enum State
    {
        Hover,
        Miss,
        Success
    }

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
        actionReady = true;
        tween = transform.DOMove(rightHoverPoint.position, cycleTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            default:
            case State.Hover:
                HoverAI();
            break;
            case State.Miss:
                MissAI();
            break;
            case State.Success:
                SuccessAI();
            break;
        }
    }

    private void HoverAI()
    {
        if (actionReady == false)
        {
            if (inClockRange)
            {
                tween.Kill();
                transform.position = successStartPoint.position;
                tween = transform.DOMove(successEndPoint.position, cycleTime).SetEase(Ease.InOutSine);
                state = State.Success;
            }
            else
            {
                tween.Kill();
                transform.position = new Vector3(transform.position.x, missHeight.position.y, transform.position.z);
                //NOTE: Add code for camera shake
                state = State.Miss;
            }
        }
    }

    private void MissAI()
    {
        missTimer += Time.deltaTime;
        if(missTimer >= maxMissTime)
        {
            missTimer = 0;
            ResetActionInput();
            transform.position = leftHoverPoint.position;
            tween = transform.DOMove(rightHoverPoint.position, cycleTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            state = State.Hover;
        }
    }

    private void SuccessAI()
    {
        successTimer += Time.deltaTime;
        if(successTimer >= maxSuccessTime)
        {
            miniGameManager.StartChangeGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inClockRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inClockRange = false;
    }

    private void ActionInput(InputAction.CallbackContext context)
    {
        if (actionReady)
        {
            actionReady = false;
            tween.Kill();
        }
    }

    private void ResetActionInput()
    {
        actionReady = true;
    }
}
