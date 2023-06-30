using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchoolController : MonoBehaviour
{
    [SerializeField] float distanceBetween = .2f;
    [SerializeField] float speed = 280;
    [SerializeField] float turnSpeed = 180;
    [SerializeField] GameObject fishPrefab;

    private Transform cityCenter;

    [Header("Fish Contained In School")]
    public List<GameObject> fishToSpawn = new List<GameObject>();
    public List<GameObject> spawnedFish = new List<GameObject>();
    float countUp = 0;

    public float verticalMoveSpeed;
    private Rigidbody leaderRigidBody;

    public float radius = 5.0f; //Size of circle
    public float angularPosition = -85f; //Where on the perimeter to spawn
    internal bool spawnLeader = false; //First Enemy in school

    private bool spawnFollowers = false;
    private Rigidbody previouslySpawned;
    private Vector3 offset;

    private void Start()
    {
        cityCenter = GameObject.FindGameObjectWithTag("AtlantisCenter").transform;

        for (int i = 0; i < fishToSpawn.Count; i++)
        {
            fishToSpawn[i] = fishPrefab;
        }
    }

    private void FixedUpdate()
    {
        if(spawnLeader)
        {
            SpawnLeader();
        }

        if (spawnFollowers)
        {
            SpawnFollowers();
        }
    }

    public void SpawnSchool(Vector3 followerOffset, float distanceBetween)
    {
        this.distanceBetween = distanceBetween;
        offset = followerOffset;
        spawnLeader = true;
    }

    public void SpawnLeader()
    {
        GameObject leader = Instantiate(fishToSpawn[0], PlaceOnCylinderPerimeter(), transform.rotation, transform);

        BHEnemyController controller = leader.GetComponent<BHEnemyController>();
        controller.currentState = EnemyState.Leader;
        controller.center = cityCenter;
            
        spawnedFish.Add(leader);
        fishToSpawn.RemoveAt(0);
       
        leaderRigidBody = spawnedFish[0].GetComponentInChildren<Rigidbody>();
        previouslySpawned = leaderRigidBody;
        spawnLeader = false;
        spawnFollowers = true;
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

            BHEnemyController controller = follower.GetComponent<BHEnemyController>();
            controller.currentState = EnemyState.Follower;
            controller.offset = offset;
            controller.center = cityCenter;
            controller.rigidBodyToFollow = previouslySpawned;
            
            previouslySpawned = controller.GetComponentInChildren<Rigidbody>();
            spawnedFish.Add(follower);
            fishToSpawn.RemoveAt(0);

            follower.GetComponentInChildren<FishPositionManager>().ClearMarkerList();
            countUp = 0;
        }

        if (fishToSpawn.Count == 0)
        {
            spawnFollowers = false;
            Destroy(GetComponent<FishSchoolController>());
        }

    }

    /**
     * Depending on radius of gameplay cylinder, place enemy on certain
     * angular postion on the cylinder
     **/
    private Vector3 PlaceOnCylinderPerimeter()
    {
        // Convert angle to radians
        float radians = angularPosition * Mathf.Deg2Rad;

        // Calculate position on the circumference
        float x = radius * Mathf.Cos(radians);
        float y = 0f;
        float z = radius * Mathf.Sin(radians);

        Vector3 cylinderPosition = new Vector3(x, y, z);

        return cylinderPosition;
    }

}
