using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ư�� ��ũ��Ʈ���� ���ο� ������ ������ �� �ִ� �޴� �ɼ��� ����
[CreateAssetMenu(menuName = "2DFlock/Behavior/Cohesion")] // MonoBehaviour�� ������ ������ �ȵ�
public class CohesionBehavior: FilteredFlockBehavior // ���� �ൿ
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //����� agent�� ������ ��ġ�� �������� ����
        if (context.Count == 0) {
            return Vector2.zero;
        }

        //��� ������ ��ġ ����� ����
        Vector2 cohesionMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= context.Count;

        //agent�� ��ġ���� offset ����
        cohesionMove -= (Vector2)agent.transform.position;
        return cohesionMove;
    }
}
