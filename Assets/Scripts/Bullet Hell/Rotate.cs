using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate : MonoBehaviour
{
    public float moveSpeed;
    float inputX;
    float inputY;
    public BHPlayerActions moveActions;
    private InputAction movePlayer;
    private Rigidbody rb;

    [Range(0.6f, .999f)]
    public float dragCoefficient = 0.95f;

    private void Awake()
    {
        moveActions = new BHPlayerActions();
        rb = GetComponent<Rigidbody>();
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
        inputX = movePlayer.ReadValue<Vector2>().x;
        inputY = movePlayer.ReadValue<Vector2>().y;
    }

    private void FixedUpdate()
    {
        // Apply movement force to the Rigidbody
        rb.AddForce(new Vector2(inputX, inputY) * moveSpeed);

        // Reduce velocity gradually when no input is given
        if (inputX == 0 && inputY == 0)
        {
            rb.velocity *= dragCoefficient; // 0.9f is the drag coefficient
        }

        Debug.Log(String.Format("InputX: {0} InputY {1}", inputX, inputY));
    }
}