using UnityEngine;
using System.Collections;
using System;

public class BHProjectile : MonoBehaviour
{
    [SerializeField] float lifeTime = 4;
    [SerializeField] GameObject bulletSizeObject;

    public bool isEnemyBullet = false;

    public Vector3 bulletDirection;
    public float bulletSize = 1f;
    public MeshRenderer meshRenderer;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem collideParticle;
    public ParticleSystem expireParticle;
    public AudioSource bulletAudio;
    public AudioClip impactSoundEffect;

    [Space(10)]
    public BHTorpedoStats projectileType;

    private float bulletDamage;
    private float bulletSpeed;
    private bool startRotate;
    private bool hasBoomed = false;
    private Transform rotation;
    private SubmarineWeaponType weaponType;
    private float vertDir;

    protected float m_SinceFired;

    protected Rigidbody m_RigidBody;
    int m_EnvironmentLayer = -1;


    // Spray/Bloom bullet
    private float spreadAmount = 0f;

    // Helix bullet
    private float verticalSpeed = 5f;           // Speed of vertical movement
    private float verticalRange = .3f;           // Range of vertical movement
    private float crissCrossTime = 0f;          // Timer for oscillation

    // Remote Explode Bullet
    BHShooterController firedFrom = null;

    void Awake()
    {
        m_EnvironmentLayer = 1 << LayerMask.NameToLayer("Environment");
        m_RigidBody = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        m_RigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        m_SinceFired = 0.0f;

    }

    void FixedUpdate()
    {
        m_SinceFired += Time.deltaTime;

        //Prevent projectile from colliding with its owner
        if(m_SinceFired > 0.2f)
        {
            m_RigidBody.detectCollisions = true;
        }

        if(!startRotate)
        {
            return;
        }
    
        switch (projectileType.projectileEnumReference)
        {
            case BHTorpedoType.multiBullet:
                MoveMultiBullet();
                break;
            case BHTorpedoType.sprayBullet:
                MoveSprayBullet();
                break;
            case BHTorpedoType.helixBullet:
                MoveHelixBullet();
                break;
            case BHTorpedoType.remoteExplosive:
                MoveMultiBullet();
                break;
            default:
                break;
        }
        
    }

    private void GetStats(Transform center, ProjectileStats stats)
    {
        vertDir = stats.verticalDirection;
        lifeTime = stats.bulletLife;
        weaponType = stats.weaponType;
        bulletSize = stats.bulletSize;
        bulletSpeed = stats.bulletSpeed;
        bulletDamage = stats.bulletDamage;

        //I dont remember Why i did this, but dont remove it
        if (vertDir == -1)
            vertDir = -.06f;
        if (vertDir == 1)
            vertDir = .06f;

        rotation = center;
        startRotate = true;

        //Set scale of meshRender (And colliders)
        bulletSizeObject.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
    }


    internal void Shoot(Transform center, ProjectileStats projInfo)
    {
        GetStats(center, projInfo);

        this.projectileType = projInfo.statsProjectileType;

        StartCoroutine(DeathDelay());
    }

    internal void ShootHelix(Transform center, ProjectileStats projInfo, float verticalSpeed, float verticalRange)
    {
        GetStats(center, projInfo);

        this.projectileType = projInfo.statsProjectileType;
        this.verticalSpeed = verticalSpeed;
        this.verticalRange = verticalRange;

        StartCoroutine(DeathDelay());
    }

    internal void ShootRemoteExplosive(Transform center, ProjectileStats projInfo, BHShooterController firedFrom)
    {
        this.firedFrom = firedFrom;
        this.firedFrom.singleProjectileActive = true;
        GetStats(center, projInfo);
        this.projectileType = projInfo.statsProjectileType;

        StartCoroutine(DeathDelayRemote());
    }

    /*
    * Helix Bullet
    */

    private void MoveMultiBullet()
    {
        transform.RotateAround(rotation.position, Vector3.up, bulletSpeed * Time.deltaTime);
    }

    private void MoveSprayBullet()
    {
        //One goes up, one goes middle, on goes down
        transform.RotateAround(rotation.position, Vector3.up, bulletSpeed * Time.deltaTime);

        if (projectileType.projectileEnumReference == BHTorpedoType.sprayBullet && (spreadAmount <= 1f))
        {
            transform.Translate(0f, vertDir / 2, 0f);
            spreadAmount += 0.1f;
        }
    }

    private void MoveHelixBullet()
    {
        //bullets weave up and down, like a sin wave

        transform.RotateAround(rotation.position, Vector3.up, bulletSpeed * Time.deltaTime);

        if (projectileType.projectileEnumReference == BHTorpedoType.helixBullet)
        {
            crissCrossTime += Time.deltaTime;

            float verticalMovement = Mathf.Sin((crissCrossTime * verticalSpeed) + (Mathf.PI / 2)) * verticalRange;

            transform.Translate(0f, verticalMovement * vertDir, 0f);
        }
    }

    /*
     * Remote Explosive Bullet
     */

    public void ManualRemoteExplode(BHShooterController triggeredFrom)
    {
        if (hasBoomed == false)
        {
            bulletSpeed = 0;
            meshRenderer.enabled = false;
            if (collideParticle != null)
            {
                collideParticle.gameObject.transform.localScale *= 10;
                collideParticle.Play();
            }
            hasBoomed = true;
            StartCoroutine(DelayAllowNewFire(.5f, triggeredFrom));
        }

        triggeredFrom.activeRemoteExplosive = null;
    }


    /*
    * Collision Management
    */

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && isEnemyBullet)
        {
            PlayerCollide(collider);
        }

        if (collider.tag == "EnemyCollider" && !isEnemyBullet)
        {

            EnemyCollide(collider);
        }
    }

    private void PlayerCollide(Collider collider)
    {
        //    collider.GetComponent<BHHealthManager>().ApplyDamage(damage);
        //    bulletAudio.clip = impactSoundEffect;
        //    spriteRenderer.enabled = false;
        //    partSystem.Play();
        //    bulletAudio.Play();
        //    StartCoroutine(DelayedDestroy());
    }

    private void EnemyCollide(Collider collider)
    {
        switch (weaponType)
        {
            case SubmarineWeaponType.explosive:
                bulletSpeed = 0;
                meshRenderer.enabled = false;
                StartCoroutine(DelayedDestroy(2f));
                collider.GetComponentInParent<BHHealthManager>().ApplyDamage(bulletDamage);
                hasBoomed = true;

                if(firedFrom != null)
                {
                    firedFrom.singleProjectileActive = false;    
                }

                break;
            case SubmarineWeaponType.retractable:
                meshRenderer.enabled = false;
                break;
            case SubmarineWeaponType.specialAbility:
                break;
        }

        if (collideParticle != null)
            collideParticle.Play();
    }

    /**
     * Enumerators
     **/


    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        //bulletSpeed /= 2;
        bulletSpeed = 0;
        meshRenderer.enabled = false;
        //m_RigidBody.useGravity = true;
        expireParticle.Play();
        StartCoroutine(DelayedDestroy(3f));
    }

    IEnumerator DeathDelayRemote()
    {
        yield return new WaitForSeconds(lifeTime);

        if (!hasBoomed)
        {
            //bulletSpeed /= 2;
            bulletSpeed = 0;
            meshRenderer.enabled = false;
            //m_RigidBody.useGravity = true;
            if (collideParticle != null)
                collideParticle.Play();

            firedFrom.singleProjectileActive = false;
            StartCoroutine(DelayedDestroy(3f));
        }
    }

    private IEnumerator DelayAllowNewFire(float destroyTime, BHShooterController triggeredFrom)
    {
        yield return new WaitForSeconds(destroyTime);
        triggeredFrom.singleProjectileActive = false;
        Destroy(gameObject);
    }

    private IEnumerator DelayedDestroy(float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }


}