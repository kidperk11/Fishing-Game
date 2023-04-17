using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class FPCam : MonoBehaviour
{
    public Camera fpsCam;
    public float mouseSenseX;
    public float mouseSenseY;
    public Vector2 controllerSenseScale;
    public FPPlayerActions cameraControls;
    public PlayerInput playerInput;
    
    //public string currentControlScheme { get; }

    public Transform orientation;
    float xRotation;
    float yRotation;
    //public Magnetism magnet;
    private InputAction moveCam;

    private void OnEnable()
    {
        moveCam = cameraControls.Player.Look;
        moveCam.Enable();
    }

    private void OnDisable()
    {
        moveCam.Disable();
    }

    private void Awake()
    {
        cameraControls = new FPPlayerActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Locks cursor to middle of screen and makes it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float inputX;
        float inputY;

        //This code will scale the controller's sensitivity to the mouse to make
        //it feel like the same speed as the mouse.
        if (playerInput.currentControlScheme == "Gamepad")
        {
            //Get a reference to the current camera input
            inputX = moveCam.ReadValue<Vector2>().x * mouseSenseX * controllerSenseScale.x;
            inputY = moveCam.ReadValue<Vector2>().y * mouseSenseY * controllerSenseScale.y;


        }
        else
        {
            //Get a reference to the current camera input
            inputX = moveCam.ReadValue<Vector2>().x * Time.deltaTime * mouseSenseX;
            inputY = moveCam.ReadValue<Vector2>().y * Time.deltaTime * mouseSenseY;
        }
        

        yRotation += inputX;

        xRotation -= inputY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if(playerInput.currentControlScheme == "Gamepad")
        {
            //Rotate camera
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

            //Rotate player
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);

            //// get the assisted angles, using the player MOVEMENT input as parameter
            //var aimAssist = magnet.AssistAim(new Vector2());

            //// add turn addition
            //var turnAddition = Quaternion.Euler(aimAssist.TurnAddition);
            //rb.MoveRotation(rb.rotation * turnAddition);

            //// add pitch addition
            //cinemachineTargetPitch += aimAssist.PitchAdditionInDegrees;
            //cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);
            //CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(cinemachineTargetPitch, 0f, 0f);
        }
        else
        {
            //Rotate camera
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

            //Rotate player
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        } 
    }
}
