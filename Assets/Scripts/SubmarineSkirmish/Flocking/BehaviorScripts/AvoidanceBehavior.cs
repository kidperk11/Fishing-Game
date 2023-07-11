using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FilteredFlockBehavior
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //If no neighbors, return no adjustment
        if (context.Count == 0)
            return Vector3.zero;

        //Add all points together and average
        Vector3 avoidanceMove = Vector3.zero;

        int nAvoid = 0;

        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {

            Vector3 closestPoint = item.gameObject.GetComponent<Collider>().ClosestPoint(agent.transform.position);
            //agent.sphere.transform.position = closestPoint;

            if (Vector3.SqrMagnitude(closestPoint - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                Debug.Log(flock.SquareAvoidanceRadius);
                nAvoid++;
                avoidanceMove += agent.transform.position - item.position;

                //Agents will not avoid if their y values are equal to each other
                if(avoidanceMove.y == 0)
                    avoidanceMove.y = Random.Range(-0.1f, 0.1f);

                avoidanceMove.x = 0;
                avoidanceMove.z = 0;
            }
        }

        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }

        return avoidanceMove;
    }
}
