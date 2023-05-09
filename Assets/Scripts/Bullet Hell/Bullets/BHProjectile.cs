using UnityEngine;
using System.Collections;

public class BHProjectile : MonoBehaviour
{

    [SerializeField] float lifeTime = 4;

    public bool isEnemyBullet = false;

    public float damage = 1f;
    public Vector3 bulletDirection;
    public float bulletSize = 1f;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem partSystem;
    public AudioSource bulletAudio;
    public AudioClip impactSoundEffect;

    private float bulletDamage;
    private float bulletSpeed;
    private bool startRotate;
    private Transform rotation;

    protected float m_SinceFired;

    protected Rigidbody m_RigidBody;
    int m_EnvironmentLayer = -1;

    void Awake()
    {
        m_EnvironmentLayer = 1 << LayerMask.NameToLayer("Environment");
        m_RigidBody = GetComponent<Rigidbody>();
        m_RigidBody.detectCollisions = false;
    }


    private void OnEnable()
    {
        m_RigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
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
        bulletSpeed /= 2;
        m_RigidBody.useGravity = true;
    }
    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public void Shoot(float z, float speed, float size, float damage, float bulletLife, Transform center)
    {
        //bulletDamage = damage;
        //bulletDirection.x = z;
        //bulletDirection.y = 0;
        //bulletDirection.z = 0;
        bulletSpeed = speed;
        bulletSize = size;
        lifeTime = bulletLife;

        //m_RigidBody.isKinematic = false;

        //m_RigidBody.detectCollisions = false;

        //if (isEnemyBullet)
        //{
        //    m_RigidBody.velocity = new Vector3(bulletDirection.x * bulletSpeed, 0, bulletDirection.z * bulletSpeed);
        //}
        //else
        //{
        //    m_RigidBody.velocity = new Vector3(
        //        (bulletDirection.x < 0) ? Mathf.Floor(bulletDirection.x) * bulletSpeed : Mathf.Ceil(bulletDirection.x) * bulletSpeed,
        //        0,
        //        (bulletDirection.z < 0) ? Mathf.Floor(bulletDirection.z) * bulletSpeed : Mathf.Ceil(bulletDirection.z) * bulletSpeed
        //    );
        //}


        // Accelerate/decelerate towards target speed
        //currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref currentVelocity, smoothTime);

        rotation = center;
        startRotate = true;
        //transform.RotateAround(center.position, Vector3.up, bulletSpeed * Time.deltaTime);

        transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);

        StartCoroutine(DeathDelay());

        StartCoroutine(DelayedDestroy());
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(partSystem != null)
        {
            if (collider.tag == "Player" && isEnemyBullet)
            {
                collider.GetComponent<BHHealthManager>().ApplyDamage(damage);
                bulletAudio.clip = impactSoundEffect;
                spriteRenderer.enabled = false;
                partSystem.Play();
                bulletAudio.Play();
                StartCoroutine(DelayedDestroy());
            }

            if (collider.tag == "EnemyCollider" && !isEnemyBullet)
            {
                collider.GetComponentInParent<BHHealthManager>().ApplyDamage(bulletDamage);

            }
        }
        else
        {
            if (collider.tag == "Player" && isEnemyBullet)
            {

                collider.GetComponent<BHHealthManager>().ApplyDamage(damage);
                Destroy(gameObject);
            }

            if (collider.tag == "EnemyCollider" && !isEnemyBullet)
            {
                collider.GetComponentInParent<BHHealthManager>().ApplyDamage(bulletDamage);
                Destroy(gameObject);
            }
        }
    }



    protected virtual void OnCollisionEnter(Collision collision)
    {

    }
}