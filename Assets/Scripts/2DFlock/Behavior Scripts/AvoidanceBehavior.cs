using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu(menuName = "2DFlock/Behavior/Avoidance")]
public class AvoidanceBehavior : FilteredFlockBehavior // ȸ�� �ൿ
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //����� agent�� ������ �������� ��ȯ���� ����
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        //��� ������ ��ġ ����� ���� 
        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0; // ȸ���ؾ��ϴ� ��
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            if(Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius) { // �̿� ��ü���� �Ÿ��� ȸ�� �ݰ溸�� ���� ���
                nAvoid++;
                avoidanceMove += (Vector2)(agent.transform.position - item.position);
            }
        }
        if (nAvoid > 0){
            avoidanceMove /= nAvoid;
        }

        return avoidanceMove;
    }
}
