using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayerNavMeshTest : MonoBehaviour
{
    public NavMeshAgent agent;
    public NavAgentCollisionManagement collisionManagement;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!collisionManagement.isRagdoll)
        {
            agent.SetDestination(player.transform.position);
        }
        
    }
}
