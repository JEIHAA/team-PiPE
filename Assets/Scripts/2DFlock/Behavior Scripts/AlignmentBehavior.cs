using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2DFlock/Behavior/Alignment")]
public class AlignmentBehavior : FilteredFlockBehavior // ���� �ൿ
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //����� agent�� ������ ���� ������ ����
        if (context.Count == 0)
        {
            return agent.transform.up;
        }

        //��� ������ ��ġ ����� ����
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
