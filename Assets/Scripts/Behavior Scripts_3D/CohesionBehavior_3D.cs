using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//특정 스크립트에서 새로운 에셋을 생성할 수 있는 메뉴 옵션을 제공
[CreateAssetMenu(menuName = "3DFlock/Behavior/Cohesion")] // MonoBehaviour를 가지고 있으면 안됨
public class CohesionBehavior_3D : FilteredFlockBehavior_3D // 응집 행동
{
    public override Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock, List<Transform> flag)
    {
        //가까운 agent가 없으면 위치를 조정하지 않음
        if (context.Count == 0) {
            return Vector3.zero;
        }

        //모든 지점의 위치 평균을 구함
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            cohesionMove += item.position;
        }
        cohesionMove /= context.Count;

        //agent의 위치에서 offset 생성
        cohesionMove -= agent.transform.position;
        return cohesionMove;
    }
}
