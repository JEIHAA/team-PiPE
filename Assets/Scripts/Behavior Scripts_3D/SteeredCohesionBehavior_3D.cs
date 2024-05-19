using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 더 부드럽고 갑작스럽지 않은 응집 이동
[CreateAssetMenu(menuName = "3DFlock/Behavior/Steered Cohesion")]
public class SteeredCohesionBehavior_3D : FilteredFlockBehavior_3D // 응집 행동
{
    Vector3 currentVelocity;
    public float agentSmoothTime = 0.5f; // 현재 상태에서 계산된 상태로 이동하는데까지 걸릴 시간 계수
        
    public override Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock, List<Transform> flag)
    {
        //가까운 agent가 없으면 위치를 조정하지 않음
        if (context.Count <= 1)
        {
            return Vector3.zero;
        }

        //모든 지점의 위치 평균을 구함
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        // 필터가 null이라면 (필터를 사용하지 않는다면) 원래의 context를 사용, 필터를 사용하고 있다면 filter.Filter로 넘겨준 agent와 context를 사용
        foreach (Transform item in filteredContext)
        {
            cohesionMove += item.position;
        }
        cohesionMove /= context.Count;

        //agent의 위치에서 offset 생성
        cohesionMove -= agent.transform.position;
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref currentVelocity, agentSmoothTime);
        return cohesionMove;
    }
}