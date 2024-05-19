using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "3DFlock/Behavior/StayInRadius")]
public class StayInRadiusBehavior_3D : FlockBehavior_3D
{
    public Vector3 center;
    public float radius = 15f;

    public override Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock, List<Transform> flag)
    {
        Vector3 centerOffset = center - agent.transform.position;
        float t = centerOffset.magnitude / radius;
        if (t < 0.9f) {
            return Vector3.zero;
        }
        return centerOffset * t * t;

    }
}
