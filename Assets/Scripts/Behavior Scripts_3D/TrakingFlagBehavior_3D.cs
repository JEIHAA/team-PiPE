using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


[CreateAssetMenu(menuName = "3DFlock/Behavior/TracePlagBehavior")]
public class TrakingFlagBehavior_3D : FilteredFlockBehavior_3D
{
    public override Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock, List<Transform> flags)
    {
        //가까운 agent가 없으면 조정값을 반환하지 않음
        if (context.Count <= 1)
        {
            return Vector3.zero;
        }

        Vector3 traceFlagMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, flags);
        foreach (Transform item in filteredContext)
        {
            traceFlagMove += agent.transform.position + item.position;
            // flag를 안봄, lookAt 해줘야함
        }
        return traceFlagMove;
    }
}
