using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BHMove : MonoBehaviour
{
    public Transform center;

    [Header("Movement Speeds")]
    public float verticalMoveSpeed;
    public float horizontalMoveSpeed;

    [Space(5)]
    [Header("Sprites")]
    public GameObject submarineSprite;

    float inputX;
    float inputY;
    public BHPlayerActions moveActions;
    private InputAction movePlayer;

    Rigidbody rigidBody;

    private void Awake()
    {
        moveActions = new BHPlayerActions();
    }

    private void OnEnable()
    {
        movePlayer = moveActions.Player.Move;
        movePlayer.Enable();
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        movePlayer.Disable();
    }

    private void Update()
    {
        CurrentInput();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void CurrentInput()
    {
        inputX = movePlayer.ReadValue<Vector2>().x;
        inputY = movePlayer.ReadValue<Vector2>().y;
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        //Limit velocity
        if (flatVelocity.magnitude > horizontalMoveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * horizontalMoveSpeed;
            rigidBody.velocity = new Vector3(limitedVelocity.x, rigidBody.velocity.y, limitedVelocity.z);
        }
    }

    private void MovePlayer()
    {
        if (inputX != 0)
            inputX = Mathf.Sign(inputX);
        if (inputY != 0)
            inputY = Mathf.Sign(inputY);

        Debug.Log(String.Format("InputX: {0} InputY {1}", inputX, inputY));

        transform.position += new Vector3(0, inputY * verticalMoveSpeed, 0);
        center.transform.Rotate(new Vector3(0, -inputX * horizontalMoveSpeed, 0));

        Vector3 scaleHolder = submarineSprite.transform.localScale;

        if(inputX > 0)
        {
            submarineSprite.transform.localScale = (new Vector3(-0.77f, scaleHolder.y, scaleHolder.z));
        }
        else if (inputX < 0)
        {
            submarineSprite.transform.localScale = (new Vector3(0.77f, scaleHolder.y, scaleHolder.z));
        }
    }
}

