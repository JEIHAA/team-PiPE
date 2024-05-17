using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ư�� ��ũ��Ʈ���� ���ο� ������ ������ �� �ִ� �޴� �ɼ��� ����
[CreateAssetMenu(menuName = "3DFlock/Behavior/Cohesion")] // MonoBehaviour�� ������ ������ �ȵ�
public class CohesionBehavior_3D : FilteredFlockBehavior_3D // ���� �ൿ
{
    public override Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock)
    {
        //����� agent�� ������ ��ġ�� �������� ����
        if (context.Count == 0) {
            return Vector3.zero;
        }

        //��� ������ ��ġ ����� ����
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            cohesionMove += item.position;
        }
        cohesionMove /= context.Count;

        //agent�� ��ġ���� offset ����
        cohesionMove -= agent.transform.position;
        return cohesionMove;
    }
}
