using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


[CreateAssetMenu(menuName = "3DFlock/Behavior/TracePlagBehavior")]
public class TrakingFlagBehavior_3D : FilteredFlockBehavior_3D
{
    [SerializeField] private List<Transform> flags;

    public override Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock)
    {
        //가까운 목표가 없으면 조정값을 반환하지 않음
        if (context.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 followMove = Vector3.zero;
        int nFollow = 0; // 도착해야하는 플래그 수
        foreach (Transform item in flags)
        {
            Debug.Log(item);
            if (Vector3.SqrMagnitude(item.position - agent.transform.position) <flock.SquareFollowRadius)
            { // 이웃 객체와의 거리가 추적 반경보다 작을 경우
                nFollow++;
                followMove += agent.transform.position + item.position;
            }
        }
        if (nFollow > 0)
        {
            followMove /= nFollow;
        }

        return followMove;
    }
}
