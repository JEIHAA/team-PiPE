using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//특정 스크립트에서 새로운 에셋을 생성할 수 있는 메뉴 옵션을 제공
[CreateAssetMenu(menuName = "2DFlock/Behavior/Cohesion")] // MonoBehaviour를 가지고 있으면 안됨
public class CohesionBehavior: FilteredFlockBehavior // 응집 행동
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //가까운 agent가 없으면 위치를 조정하지 않음
        if (context.Count == 0) {
            return Vector2.zero;
        }

        //모든 지점의 위치 평균을 구함
        Vector2 cohesionMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= context.Count;

        //agent의 위치에서 offset 생성
        cohesionMove -= (Vector2)agent.transform.position;
        return cohesionMove;
    }
}
