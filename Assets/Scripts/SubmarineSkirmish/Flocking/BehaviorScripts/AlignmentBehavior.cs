using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]
public class AlignmentBehavior : FilteredFlockBehavior
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //If no neighbors, maintain current alignment
        if (context.Count == 0)
            return agent.transform.right;

        //Add all points together and average
        Vector3 alighmentMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            //Possibly transform.forward for 3d
            alighmentMove += item.transform.position;
        }

        alighmentMove /= context.Count;

        return alighmentMove;
    }
}
