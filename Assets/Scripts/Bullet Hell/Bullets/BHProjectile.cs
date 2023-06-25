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
    public ProjectileType projectileType;

    private float bulletDamage;
    private float bulletSpeed;
    private bool startRotate;
    private Transform rotation;
    private SubmarineWeapons weaponType;
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
            case ProjectileType.ProjectilesEnum.multiBullet:
                MoveMultiBullet();
                break;
            case ProjectileType.ProjectilesEnum.sprayBullet:
                MoveSprayBullet();
                break;
            case ProjectileType.ProjectilesEnum.helixBullet:
                MoveHelixBullet();
                break;
            case ProjectileType.ProjectilesEnum.remoteExplosive:
                MoveMultiBullet();
                break;
            default:
                break;
        }
        
    }

    private void MoveMultiBullet()
    {
        transform.RotateAround(rotation.position, Vector3.up, bulletSpeed * Time.deltaTime);
    }

    private void MoveSprayBullet()
    {
        //One goes up, one goes middle, on goes down
        transform.RotateAround(rotation.position, Vector3.up, bulletSpeed * Time.deltaTime);

        if (projectileType.projectileEnumReference == ProjectileType.ProjectilesEnum.sprayBullet && (spreadAmount <= 1f))
        {
            transform.Translate(0f, vertDir / 2, 0f);
            spreadAmount += 0.1f;
        }
    }


    private void MoveHelixBullet()
    {
        //bullets weave up and down, like a sin wave

        transform.RotateAround(rotation.position, Vector3.up, bulletSpeed * Time.deltaTime);

        if (projectileType.projectileEnumReference == ProjectileType.ProjectilesEnum.helixBullet)
        {
            crissCrossTime += Time.deltaTime;

            float verticalMovement = Mathf.Sin((crissCrossTime * verticalSpeed) + (Mathf.PI / 2)) * verticalRange;

            transform.Translate(0f, verticalMovement * vertDir, 0f);
        }
    }

    internal void Shoot(Transform center, ProjectileStats projInfo)
    {

        vertDir             = projInfo.verticalDirection;
        lifeTime            = projInfo.bulletLife;
        weaponType          = projInfo.weaponType;
        bulletSize          = projInfo.bulletSize;
        bulletSpeed         = projInfo.bulletSpeed;
        bulletDamage        = projInfo.bulletDamage;
        this.projectileType = projInfo.statsProjectileType;

        //I dont remember Why i did this, but dont remove it
        if (vertDir == -1)
            vertDir = -.06f;
        if (vertDir == 1)
            vertDir = .06f;

        rotation = center;
        startRotate = true;

        //Set scale of meshRender (And colliders)
        bulletSizeObject.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);

        StartCoroutine(DeathDelay());
    }

    internal void ShootHelix(Transform center, ProjectileStats projInfo, float verticalSpeed, float verticalRange)
    {
        vertDir = projInfo.verticalDirection;
        lifeTime = projInfo.bulletLife;
        weaponType = projInfo.weaponType;
        bulletSize = projInfo.bulletSize;
        bulletSpeed = projInfo.bulletSpeed;
        bulletDamage = projInfo.bulletDamage;
        this.projectileType = projInfo.statsProjectileType;
        this.verticalSpeed = verticalSpeed;
        this.verticalRange = verticalRange;

        //I dont remember Why i did this, but dont remove it
        if (vertDir == -1)
            vertDir = -.06f;
        if (vertDir == 1)
            vertDir = .06f;

        rotation = center;
        startRotate = true;

        //Set scale of meshRender (And colliders)
        bulletSizeObject.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);

        StartCoroutine(DeathDelay());
    }

    internal void ShootRemoteExplosive(Transform center, ProjectileStats projInfo)
    {
        vertDir = projInfo.verticalDirection;
        lifeTime = projInfo.bulletLife;
        weaponType = projInfo.weaponType;
        bulletSize = projInfo.bulletSize;
        bulletSpeed = projInfo.bulletSpeed;
        bulletDamage = projInfo.bulletDamage;
        this.projectileType = projInfo.statsProjectileType;

        //I dont remember Why i did this, but dont remove it
        if (vertDir == -1)
            vertDir = -.06f;
        if (vertDir == 1)
            vertDir = .06f;

        rotation = center;
        startRotate = true;

        //Set scale of meshRender (And colliders)
        bulletSizeObject.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);

        StartCoroutine(DeathDelay());
    }

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
            case SubmarineWeapons.explosive:
                bulletSpeed = 0;
                meshRenderer.enabled = false;
                StartCoroutine(DelayedDestroy(2f));
                collider.GetComponentInParent<BHHealthManager>().ApplyDamage(bulletDamage);
                break;
            case SubmarineWeapons.retractable:
                meshRenderer.enabled = false;
                break;
            case SubmarineWeapons.specialAbility:
                break;
        }

        if (collideParticle != null)
            collideParticle.Play();
    }

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

    private IEnumerator DelayedDestroy(float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }


}