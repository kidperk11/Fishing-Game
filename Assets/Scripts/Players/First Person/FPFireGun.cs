using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPFireGun : MonoBehaviour
{
    public FPPlayerActions playerActions;
    public Rigidbody playerRB;
    private InputAction fireGun;
    public Camera fpsCam;
    public Transform gunFireTransform;
    public bool readyToFire;

    public int gunDamage;

    public GameObject bulletImpact;

    private void OnEnable()
    {
        fireGun = playerActions.Player.Gun;
        fireGun.Enable();

        fireGun.performed += Fire;
    }

    private void OnDisable()
    {
        fireGun.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        readyToFire = true;
    }

    private void Awake()
    {
        playerActions = new FPPlayerActions();
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

            if (Physics.Raycast(ray, out hit))
            {
                EnemyHealthAndQTE enemy = hit.transform.GetComponent<EnemyHealthAndQTE>();
                if(enemy != null)
                {
                    enemy.TakeDamage(gunDamage);
                }
                Debug.Log("Hit Object: " + hit.transform.gameObject );
                GameObject impact = Instantiate(bulletImpact, hit.point, Quaternion.identity);
                impact.transform.LookAt(this.transform);
            }
            readyToFire = false;
            StartCoroutine(GunCooldown());
        }
    }

    public void ResetFire()
    {
        readyToFire = true;
    }

    public IEnumerator GunCooldown()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        readyToFire = true;
    }
}
