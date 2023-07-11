using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Stay In Radius")]
public class StayInRadiusBehavior : FilteredFlockBehavior
{
    public Vector3 center;
    public float radius = 15f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        center.y = agent.transform.position.y;
        Vector3 desiredPosition = (agent.transform.position - center).normalized * radius + center;

        return desiredPosition - agent.transform.position;
    }
}
