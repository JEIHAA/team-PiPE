using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2DFlock/Behavior/Alignment")]
public class AlignmentBehavior : FilteredFlockBehavior // 정렬 행동
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //가까운 agent가 없으면 현재 정렬을 유지
        if (context.Count == 0)
        {
            return agent.transform.up;
        }

        //모든 지점의 위치 평균을 구함
        Vector2 alingmentMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            alingmentMove += (Vector2)item.transform.up;
        }
        alingmentMove /= context.Count;

        return alingmentMove;
    }
}
