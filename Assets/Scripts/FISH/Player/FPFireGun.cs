using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class FPFireGun : MonoBehaviour
{
    [Header("External References")]
    public FPPlayerActions playerActions;
    public Rigidbody playerRB;
    public GameObject orientation;
    private InputAction fireGun;
    public Camera fpsCam;
    public Transform gunFireTransform;
    public TextMeshProUGUI hudBulletText;
    public GameObject bulletImpact;

    [Header("Gun Control Variables")]
    public bool readyToFire;
    public int gunDamage;
    
    [Header("Prefabs for Special Bullets")]
    public GameObject pufferBombPrefab;
    public GameObject urchinBoulderPrefab;
    public string specialBullet;

    [Header("Debug Tools")]
    public bool keepSpecialBullet;

    

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
        //specialBullet = null;
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
        if (readyToFire && specialBullet == null)
        {
            Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
                if(enemy != null)
                {
                    enemy.TakeDamage(gunDamage);
                }
                else if (hit.transform.gameObject.CompareTag("Shootable"))
                {
                    Shootable shootable = hit.transform.GetComponent<Shootable>();
                    if (!shootable.shot)
                    {
                        shootable.Activate();
                    }
                    else
                    {
                        shootable.Deactivate();
                    }
                }
                Debug.Log("Hit Object: " + hit.transform.gameObject );
                GameObject impact = Instantiate(bulletImpact, hit.point, Quaternion.identity);
                impact.transform.LookAt(this.transform);
            }
            readyToFire = false;
            StartCoroutine(GunCooldown());
        }else if(readyToFire && specialBullet != null)
        {
            //Each of these cases will fire the bullet with a different set of circumstances
            //depending on what type of bullet is stored in the "specialBullet" string.
            if(specialBullet == "puffer")
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

                Vector3 directionWithoutSpread = targetPoint - gunFireTransform.position;
                SpecialPufferBulletAI bulletInstance = Instantiate(pufferBombPrefab, gunFireTransform.position, Quaternion.identity).gameObject.GetComponent<SpecialPufferBulletAI>();
                bulletInstance.gameObject.transform.forward = directionWithoutSpread.normalized;
                //bulletInstance.rb.AddForce(directionWithoutSpread.normalized * bulletInstance.bulletSpeed, ForceMode.Impulse);
            }
            else if (specialBullet == "urchin")
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

                Vector3 directionWithoutSpread = targetPoint - gunFireTransform.position;
                SpecialUrchinBulletAI bulletInstance = Instantiate(urchinBoulderPrefab, gunFireTransform.position, Quaternion.identity).gameObject.GetComponentInChildren<SpecialUrchinBulletAI>();
                bulletInstance.gameObject.transform.forward = orientation.transform.forward;
                //bulletInstance.rb.AddForce(directionWithoutSpread.normalized * bulletInstance.bulletSpeed, ForceMode.Impulse);
            }

            if (!keepSpecialBullet)
            {
                specialBullet = null;
                hudBulletText.text = new string("No special bullet active.");
            }
            
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

    public void SetSpecialBullet(string bulletType)
    {
        specialBullet = bulletType;
        hudBulletText.text = new string(specialBullet + " bullet is now active.");
    }
}
