using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu(menuName = "3DFlock/Behavior/Avoidance")]
public class AvoidanceBehavior_3D : FilteredFlockBehavior_3D // ȸ�� �ൿ
{
    public override Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock)
    {
        //����� agent�� ������ �������� ��ȯ���� ����
        if (context.Count == 0)
        {
            return Vector3.zero;
        }

        //��� ������ ��ġ ����� ���� 
        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0; // ȸ���ؾ��ϴ� ��
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            if(Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius) { // �̿� ��ü���� �Ÿ��� ȸ�� �ݰ溸�� ���� ���
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
