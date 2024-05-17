using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "3DFlock/Behavior/Alignment")]
public class AlignmentBehavior_3D : FilteredFlockBehavior_3D // 정렬 행동
{
    public override Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock)
    {
        //가까운 agent가 없으면 현재 정렬을 유지
        if (context.Count == 0)
        {
            return agent.transform.forward;
        }

        //모든 지점의 위치 평균을 구함
        Vector3 alingmentMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            alingmentMove += item.transform.forward;
        }
        alingmentMove /= context.Count;

        return alingmentMove;
    }
}
