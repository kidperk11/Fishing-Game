using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public Transform edgeofRadius;
    public Transform cityCenter;

    [Space(10)]
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;

    [Range(1, 500)]
    public int startingCount = 250;
    const float AGENT_DENSITY = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 10f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.25f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            //FlockAgent newAgent = Instantiate(
            //    agentPrefab,
            //    Random.insideUnitCircle * startingCount * AGENT_DENSITY,
            //    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
            //    transform
            //    );

            FlockAgent newAgent = Instantiate(
                agentPrefab,
                edgeofRadius.transform.position,
                Quaternion.identity,
                transform
                );

            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            newAgent.cityCenter = cityCenter;
            agents.Add(newAgent);
        }
    }

    void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            Vector3 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach (Collider collider in contextColliders)
        {
            if(collider != agent.AgentCollider)
            {
                context.Add(collider.transform);
            }
        }

        return context;
    }
}
