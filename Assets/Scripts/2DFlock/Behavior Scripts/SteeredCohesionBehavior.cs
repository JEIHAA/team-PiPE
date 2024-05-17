using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� �ε巴�� ���۽����� ���� ���� �̵�
[CreateAssetMenu(menuName = "2DFlock/Behavior/Steered Cohesion")]
public class SteeredCohesionBehavior : FilteredFlockBehavior // ���� �ൿ
{
    Vector2 currentVelocity;
    public float agentSmoothTime = 0.5f; // ���� ���¿��� ���� ���·� �̵��ϴµ����� �ɸ� �ð� ���
        
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //����� agent�� ������ ��ġ�� �������� ����
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        //��� ������ ��ġ ����� ����
        Vector2 cohesionMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        // ���Ͱ� null�̶�� (���͸� ������� �ʴ´ٸ�) ������ context�� ���, ���͸� ����ϰ� �ִٸ� filter.Filter�� �Ѱ��� agent�� context�� ���
        foreach (Transform item in filteredContext)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= context.Count;

        //agent�� ��ġ���� offset ����
        cohesionMove -= (Vector2)agent.transform.position;
        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime);
        return cohesionMove;
    }
}