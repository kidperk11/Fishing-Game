using UnityEngine;
using System.Collections;


public class BHProjectile : MonoBehaviour
{

    [SerializeField] float lifeTime = 4;

    public bool isEnemyBullet = false;

    public float damage = 1f;
    public Vector3 bulletDirection;
    public float bulletSize = 1f;
    public MeshRenderer meshRenderer;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem partSystem;
    public AudioSource bulletAudio;
    public AudioClip impactSoundEffect;

    private float bulletDamage;
    private float bulletSpeed;
    private bool startRotate;
    private Transform rotation;
    private SubmarineWeapons weaponType;

    protected float m_SinceFired;

    protected Rigidbody m_RigidBody;
    int m_EnvironmentLayer = -1;

    void Awake()
    {
        m_EnvironmentLayer = 1 << LayerMask.NameToLayer("Environment");
        m_RigidBody = GetComponent<Rigidbody>();
        //m_RigidBody.detectCollisions = false;
    }


    private void OnEnable()
    {
        m_RigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //m_RigidBody.isKinematic = true;
        m_SinceFired = 0.0f;

    }

    void FixedUpdate()
    {
        m_SinceFired += Time.deltaTime;

        if(m_SinceFired > 0.2f)
        {
            m_RigidBody.detectCollisions = true;
        }

        if(startRotate)
        {
            transform.RotateAround(rotation.position, Vector3.up, bulletSpeed * Time.deltaTime);
        }
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        //bulletSpeed /= 2;
        m_RigidBody.useGravity = true;
        StartCoroutine(DelayedDestroy(3f));
    }
    private IEnumerator DelayedDestroy(float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    public void Shoot(float z, float speed, float size, float damage, float bulletLife, Transform center, SubmarineWeapons type)
    {
        weaponType = type;
        bulletSpeed = speed;
        bulletSize = size;
        lifeTime = bulletLife;
        bulletDamage = damage;

        rotation = center;
        startRotate = true;

        transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);

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

        if (partSystem != null)
            partSystem.Play();
    }



    protected virtual void OnCollisionEnter(Collision collision)
    {

    }
}