using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUrchinBulletAI : MonoBehaviour
{
    [Header("External References")]
    public Rigidbody rb;
    public OnTriggerDetector3D detector;

    [Header("Attack Variables")]
    public int bulletDamage;
    public float bulletSpeed;
    public float knockbackSpeed;
    private List<EnemyHealth> hitEnemies = new List<EnemyHealth>();

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Force);

        if (detector.CheckIfTagDetected("Wall") || detector.CheckIfTagDetected("Ground"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if(enemy == null)
            {
                enemy = collision.gameObject.GetComponentInParent<EnemyHealth>();
            }
            Debug.Log("Enemy has been hit by Urchin Bullet");
            if (enemy != null)
            {
                Debug.Log("Enemy has been hit by Urchin Bullet, and EnemyHealth has been obtained");
                bool alreadyHit = false;
                if(hitEnemies != null)
                {
                    foreach (EnemyHealth hitEnemy in hitEnemies)
                    {
                        if (hitEnemy == enemy)
                        {
                            alreadyHit = true;
                        }
                    }
                }
                
                if (!alreadyHit)
                {
                    enemy.TakeDamage(bulletDamage);
                    Vector3 contactDirection = collision.gameObject.transform.position - this.transform.position;
                    enemy.TakeKnockback(contactDirection, knockbackSpeed);
                    hitEnemies.Add(enemy);
                }
            }
        }
    }
}
