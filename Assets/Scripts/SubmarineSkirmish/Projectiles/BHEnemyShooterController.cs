using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class BHEnemyShooterController : MonoBehaviour
    {
        private BHEnemyManager enemyController;

        [Tooltip("List of possible Shooters.")]
        [SerializeField]
        public List<BHShooter> shooters = new List<BHShooter>();
        public bool hasEnemyController;
        public GameObject bulletSpawnTransform;

        private GameObject player;
        private float startTime;
        private float rotationTime = 1.0f;

        public bool coolDownAttack { set { m_CoolDownAttack = value; } }
        private bool m_CoolDownAttack = false;

    private void OnEnable()
        {
            if (!hasEnemyController)
            {
                startTime = Time.time;
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }

        private void Start()
        {
            if (hasEnemyController)
            {
                enemyController = GetComponent<BHEnemyManager>();
            }
        }

        public void Attack()
        {

            int shooterNumber = UnityEngine.Random.Range(0, shooters.Count);

            if (shooters[shooterNumber].shooterScriptableObject.multiDirection)
            {
                FireMultiBullet(0, shooterNumber);
                FireMultiBullet(1, shooterNumber);
                FireMultiBullet(2, shooterNumber);
                FireMultiBullet(3, shooterNumber);
            }
            else
            {
                FireSingleBullet(0, shooterNumber);
            }
        }

        public void FireSingleBullet(int bulletNumber, int shooterNumber)
        {

            Debug.Log(bulletNumber);

            Vector3 playerVectors = new Vector3();
            if (hasEnemyController)
            {
                //playerVectors = enemyController.GetPlayerVectors();
            }
            else
            {
                playerVectors = GetPlayerVectors();
            }

            GameObject bullet = shooters[shooterNumber].shooterScriptableObject.BulletPrefab;
            bullet = Instantiate(bullet, bulletSpawnTransform.transform.position + Vector3.up * 1f, transform.rotation);
            bullet.GetComponent<BHProjectile>().isEnemyBullet = true;
            Shoot(shooterNumber, bullet, playerVectors.x, playerVectors.z, bulletNumber);

            if (hasEnemyController)
            {
                StartCoroutine(CoolDown());
            }
        }

        private Vector3 GetPlayerVectors()
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            float radialComplete = (Time.time - startTime) / rotationTime;

            Vector3 distanceSphere = transform.position + (transform.forward * distanceFromPlayer);
            Vector3 currentLookRel = distanceSphere - transform.position;
            Vector3 playerLookRel = player.transform.position - transform.position;
            Vector3 playerVectors = Vector3.Slerp(currentLookRel, playerLookRel, radialComplete).normalized;

            return playerVectors;
        }

        public void FireMultiBullet(int bulletNumber, int shooterNumber)
        {

            Vector3 playerVectors = Vector3.zero;
            if (hasEnemyController)
            {
                //playerVectors = enemyController.GetPlayerVectors();
            }
            else
            {
                playerVectors = GetPlayerVectors();
            }

            GameObject bullet = shooters[shooterNumber].shooterScriptableObject.BulletPrefab;
            bullet = Instantiate(bullet, bulletSpawnTransform.transform.position + Vector3.up * 1f, transform.rotation);
            bullet.GetComponent<BHProjectile>().isEnemyBullet = true;
            Shoot(shooterNumber, bullet, playerVectors.x, playerVectors.z, bulletNumber);

            if (hasEnemyController)
            {
                StartCoroutine(CoolDown());
            }
        }

        
        private void Shoot(int shooterNum, GameObject bullet, float playerX, float playerZ, int bulletNumber)
        {
            //BRING PREPARE BULLET INTO HERE
            shooters[shooterNum].PrepareBullet(bullet, playerX, playerZ, bulletNumber);
        }


        private IEnumerator CoolDown()
        {
            //enemyController.coolDownAttack = true;
            yield return new WaitForSeconds(shooters[0].shooterScriptableObject.fireDelay);
            //enemyController.coolDownAttack = false;
        }
    }
