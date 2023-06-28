using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchoolController : MonoBehaviour
{
    [SerializeField] float distanceBetween = .2f;
    [SerializeField] float speed = 280;
    [SerializeField] float turnSpeed = 180;

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

        LeaderMovement();

        //Follow the leader :^)
        if (spawnedFish.Count > 1)
        {
            FollowerMovement();
        }
    }

    void LeaderMovement()
    {
        leaderRigidBody.velocity = spawnedFish[0].transform.right * speed * Time.deltaTime;

        crissCrossTime += Time.deltaTime;
        float verticalMovement = Mathf.Sin((crissCrossTime * verticalSpeed) + (Mathf.PI / 2)) * verticalRange;
        leaderRigidBody.AddForce(new Vector2(0, verticalMovement) * verticalMoveSpeed);
    }

    void FollowerMovement()
    {
        for (int i = 1; i < spawnedFish.Count; i++)
        {
            FishPositionManager positions = spawnedFish[i - 1].GetComponent<FishPositionManager>();
            spawnedFish[i].transform.position = positions.markerList[0].position;
            spawnedFish[i].transform.rotation = positions.markerList[0].rotation;
            positions.markerList.RemoveAt(0);
        }  
    }

    private void SpawnLeader()
    {
        if (spawnedFish.Count == 0)
        {
            GameObject leader = Instantiate(fishToSpawn[0], transform.position, transform.rotation, transform);
            spawnedFish.Add(leader);
            fishToSpawn.RemoveAt(0);
        }

        leaderRigidBody = spawnedFish[0].GetComponent<Rigidbody>();
    }

    private void SpawnFollowers()
    {
        if (spawnedFish.Count == 0)
            return;

        GameObject previousFish = spawnedFish[spawnedFish.Count - 1];
        FishPositionManager swimPositions = null;

        if (countUp == 0)
        {

            swimPositions = previousFish.GetComponent<FishPositionManager>();
            swimPositions.ClearMarkerList();
        }

        countUp += Time.deltaTime;

        if (countUp >= distanceBetween)
        {
            if (swimPositions == null)
            {
                swimPositions = previousFish.GetComponent<FishPositionManager>();
            }

            GameObject follower = Instantiate(fishToSpawn[0], swimPositions.markerList[0].position, swimPositions.markerList[0].rotation, transform);

            spawnedFish.Add(follower);
            fishToSpawn.RemoveAt(0);
            follower.GetComponent<FishPositionManager>().ClearMarkerList();
            countUp = 0;
        }
    }

}
