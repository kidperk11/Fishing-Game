using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHitEffect : MonoBehaviour
{
    public float lifeTimer;
    public float maxLifeTime;
    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = maxLifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
