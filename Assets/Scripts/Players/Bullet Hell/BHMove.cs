using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BHMove : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float verticalMoveSpeed;
    public float horizontalMoveSpeed;

    float inputX;
    float inputY;
    public BHPlayerActions moveActions;
    private InputAction movePlayer;

    private void Awake()
    {
        moveActions = new BHPlayerActions();
    }

    private void OnEnable()
    {
        movePlayer = moveActions.Player.Move;
        movePlayer.Enable();
    }

    private void OnDisable()
    {
        movePlayer.Disable();
    }

    private void Update()
    {
        CurrentInput();
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

    private void MovePlayer()
    {
        if (inputX != 0)
            inputX = Mathf.Sign(inputX);
        if (inputY != 0)
            inputY = Mathf.Sign(inputY);

        Debug.Log(String.Format("InputX: {0} InputY {1}", inputX, inputY));

        transform.position += new Vector3(0, inputY * verticalMoveSpeed, 0);
        transform.Rotate(new Vector3(0, -inputX * horizontalMoveSpeed, 0));
    }
}

