using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


[CreateAssetMenu(menuName = "3DFlock/Behavior/TracePlagBehavior")]
public class TrakingFlagBehavior_3D : FilteredFlockBehavior_3D
{
    public override Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock, List<Transform> flags)
    {
        //����� agent�� ������ �������� ��ȯ���� ����
        if (context.Count <= 1)
        {
            return Vector3.zero;
        }

        Vector3 traceFlagMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, flags);
        foreach (Transform item in filteredContext)
        {
            traceFlagMove += agent.transform.position + item.position;
            // flag�� �Ⱥ�, lookAt �������
        }
        return traceFlagMove;
    }
}
