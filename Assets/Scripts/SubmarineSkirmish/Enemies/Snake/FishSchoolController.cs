using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchoolController : MonoBehaviour
{
    [SerializeField] float distanceBetween = .2f;
    [SerializeField] float speed = 280;
    [SerializeField] float turnSpeed = 180;
    [SerializeField] Transform cityCenter;

    [Header("Fish Contained In School")]
    [SerializeField] List<GameObject> fishToSpawn = new List<GameObject>();
    public List<GameObject> spawnedFish = new List<GameObject>();
    float countUp = 0;

    // Swimming/Bobbing movement
    public float verticalSpeed = 5f;           // Speed of vertical movement
    public float verticalRange = .3f;           // Range of vertical movement
    private float crissCrossTime = 0f;          // Timer for oscillation

    public float verticalMoveSpeed;
    private Rigidbody leaderRigidBody;

    public bool Manual;
    public float radius = 5.0f;
    public float angle = -85f;
    public bool spawnFollowers = true;

    private void Start()
    {
        SpawnLeader();
    }

    private void FixedUpdate()
    {
        if (fishToSpawn.Count > 0)
        {
            SpawnFollowers();
        }

        //Follow the leader :^)
        if (spawnedFish.Count > 1)
        {
            FollowerMovement();
        }
    }

    private void SpawnLeader()
    {
        if (spawnedFish.Count == 0)
        {
            GameObject leader = Instantiate(fishToSpawn[0], transform.position, transform.rotation, transform);
            PlaceOnCircle(leader);
            BHEnemyController controller = null;
            controller = leader.GetComponent<BHEnemyController>();
            controller.currentState = EnemyState.Leader;
            controller.center = cityCenter;
            controller.vertical = true;
            controller.horizontal = true;

            spawnedFish.Add(leader);
            fishToSpawn.RemoveAt(0);
        }

        leaderRigidBody = spawnedFish[0].GetComponentInChildren<Rigidbody>();
    }

    void FollowerMovement()
    {
        for (int i = 1; i < spawnedFish.Count; i++)
        {
            FishPositionManager positions = spawnedFish[i - 1].GetComponentInChildren<FishPositionManager>();
            spawnedFish[i].transform.position = positions.markerList[0].position;
            spawnedFish[i].transform.rotation = positions.markerList[0].rotation;
            positions.markerList.RemoveAt(0);
        }
    }

    private void SpawnFollowers()
    {
        if (spawnedFish.Count == 0)
            return;

        GameObject previousFish = spawnedFish[spawnedFish.Count - 1];
        FishPositionManager swimPositions = null;

        if (countUp == 0)
        {
            swimPositions = previousFish.GetComponentInChildren<FishPositionManager>();
            swimPositions.ClearMarkerList();
        }

        countUp += Time.deltaTime;

        if (countUp >= distanceBetween)
        {
            if (swimPositions == null)
            {
                swimPositions = previousFish.GetComponentInChildren<FishPositionManager>();
            }

            GameObject follower = Instantiate(fishToSpawn[0], swimPositions.markerList[0].position, swimPositions.markerList[0].rotation, transform);
            BHEnemyController controller = null;
            controller = follower.GetComponent<BHEnemyController>();
            controller.currentState = EnemyState.Follower;
            controller.center = cityCenter;

            spawnedFish.Add(follower);
            fishToSpawn.RemoveAt(0);

            follower.GetComponentInChildren<FishPositionManager>().ClearMarkerList();
            countUp = 0;
        }
    }



    private void PlaceOnCircle(GameObject enemy)
    {
        // Convert angle to radians
        float radians = angle * Mathf.Deg2Rad;

        // Calculate position on the circumference
        float x = radius * Mathf.Cos(radians);
        float y = 0f;
        float z = radius * Mathf.Sin(radians);

        // Set the object's position
        enemy.transform.position = new Vector3(x, y, z);
    }

}
