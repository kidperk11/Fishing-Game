using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireHarpoon : MonoBehaviour
{
    private HarpoonController harpoonInstance;
    public FPPlayerActions playerActions;
    private InputAction fireHarpoon;
    private InputAction reelHarpoon;
    public Camera fpsCam;
    public QTETickerController ticker;

    public GameObject harpoon;
    public Transform harpoonSpawnPoint;

    bool readyToFire;
    bool readyToReel;
    

    private void OnEnable()
    {
        fireHarpoon = playerActions.Player.Fire;
        fireHarpoon.Enable();
        reelHarpoon = playerActions.Player.Reel;
        reelHarpoon.Enable();


        //NOTE: Uncomment this code to add the functions for jump and reel
        reelHarpoon.performed += Reel;
        fireHarpoon.performed += Fire;
    }

    private void OnDisable()
    {
        fireHarpoon.Disable();
        reelHarpoon.Disable();
    }

    private void Awake()
    {
        playerActions = new FPPlayerActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        readyToFire = true;
        readyToReel = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (readyToFire)
        {

            Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(75); //This is just a point 75m from the player's LOS
            }

            Vector3 directionWithoutSpread = targetPoint - harpoonSpawnPoint.position;

            harpoonInstance = Instantiate(harpoon, harpoonSpawnPoint.position, Quaternion.identity).GetComponent<HarpoonController>();
            harpoonInstance.gameObject.transform.forward = directionWithoutSpread.normalized;
            harpoonInstance.rb.AddForce(directionWithoutSpread.normalized * harpoonInstance.harpoonSpeed, ForceMode.Impulse);
            harpoonInstance.harpoonGun = this;
            readyToFire = false;
            Debug.Log("Harpoon has been fired");
        }
        else
        {
            Debug.Log("Cannot fire harpoon, value of readyToFire is: " + readyToFire);
        }
    }

    private void Reel(InputAction.CallbackContext context)
    {
        Debug.Log("Now attempting reel...");
        Debug.Log("readyToReel Value: " + readyToReel);
        Debug.Log("ticker.currentActivationBox Value: " + ticker.currentActivationBox);

        if (readyToReel)
        {
            if(ticker.currentActivationBox != null)
            {
                Debug.Log("Reel input successful. Sending data to ticker.");
                ticker.SuccessfulInput();
            }
            else { ticker.FailTicker(); }
        }
    }

    public void ResetFire()
    {
        readyToFire = true;
    }

    public void ResetReel()
    {
        readyToReel = false;
    }

    public void ActivateReel()
    {
        readyToReel = true;
    }
    
}
