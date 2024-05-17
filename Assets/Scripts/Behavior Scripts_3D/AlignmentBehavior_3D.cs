using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "3DFlock/Behavior/Alignment")]
public class AlignmentBehavior_3D : FilteredFlockBehavior_3D // ���� �ൿ
{
    public override Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock)
    {
        //����� agent�� ������ ���� ������ ����
        if (context.Count == 0)
        {
            return agent.transform.forward;
        }

        //��� ������ ��ġ ����� ����
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
