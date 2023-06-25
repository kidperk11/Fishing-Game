using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FPSFlatDamageZone : MonoBehaviour
{
    [SerializeField] int bulletDamage;
    public float lifeTimer;
    public float maxLifeTime;
    public Vector3 initialScale;
    public Vector3 endScale;
    

    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = maxLifeTime;
        transform.DOScale(endScale, maxLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealthAndQTE enemyHealth = other.gameObject.GetComponent<EnemyHealthAndQTE>();
            enemyHealth.TakeDamage(bulletDamage);
        }
    }
}
