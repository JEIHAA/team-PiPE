using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu(menuName = "3DFlock/Behavior/Avoidance")]
public class AvoidanceBehavior_3D : FilteredFlockBehavior_3D // 회피 행동
{
    public override Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock, List<Transform> flags)
    {
        //가까운 agent가 없으면 조정값을 반환하지 않음
        if (context.Count <= 1)
        {
            return Vector3.zero;
        }

        //모든 지점의 위치 평균을 구함 
        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0; // 회피해야하는 수
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            if(Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius) { // 이웃 객체와의 거리가 회피 반경보다 작을 경우
                nAvoid++;
                avoidanceMove += agent.transform.position - item.position;
            }
        }
        if (nAvoid > 0){
            avoidanceMove /= nAvoid;
        }

        return avoidanceMove;
    }
}
