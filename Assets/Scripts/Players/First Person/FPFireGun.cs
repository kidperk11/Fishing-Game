using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
    public TextMeshProUGUI hudBulletText;
    

    [Header("Prefabs for different bullets")]
    public GameObject pufferBombPrefab;
    public string specialBullet;

    

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
        specialBullet = null;
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
        }else if(readyToFire && specialBullet != null)
        {
            //Each of these cases will fire the bullet with a different set of circumstances
            //depending on what type of bullet is stored in the "specialBullet" string.
            if(specialBullet == "puffer")
            {
                Instantiate(pufferBombPrefab, gunFireTransform.position, Quaternion.identity);
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
