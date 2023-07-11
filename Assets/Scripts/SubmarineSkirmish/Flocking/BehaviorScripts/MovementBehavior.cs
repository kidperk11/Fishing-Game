using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Movement")]
public class MovementBehavior : FilteredFlockBehavior
{


    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //If no neighbors, maintain current alignment
        if (context.Count == 0)
        {
            Vector3 radiusAdust = (agent.transform.position - agent.cityCenter.position).normalized * 5f + agent.cityCenter.position;
        }

        Vector3 rotationMove = Vector3.zero;


        return rotationMove;
    }
}
