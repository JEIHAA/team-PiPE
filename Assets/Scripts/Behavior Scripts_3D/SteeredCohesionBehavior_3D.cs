using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� �ε巴�� ���۽����� ���� ���� �̵�
[CreateAssetMenu(menuName = "3DFlock/Behavior/Steered Cohesion")]
public class SteeredCohesionBehavior_3D : FilteredFlockBehavior_3D // ���� �ൿ
{
    Vector3 currentVelocity;
    public float agentSmoothTime = 0.5f; // ���� ���¿��� ���� ���·� �̵��ϴµ����� �ɸ� �ð� ���
        
    public override Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock)
    {
        //����� agent�� ������ ��ġ�� �������� ����
        if (context.Count == 0)
        {
            return Vector3.zero;
        }

        //��� ������ ��ġ ����� ����
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        // ���Ͱ� null�̶�� (���͸� ������� �ʴ´ٸ�) ������ context�� ���, ���͸� ����ϰ� �ִٸ� filter.Filter�� �Ѱ��� agent�� context�� ���
        foreach (Transform item in filteredContext)
        {
            cohesionMove += item.position;
        }
        cohesionMove /= context.Count;

        //agent�� ��ġ���� offset ����
        cohesionMove -= agent.transform.position;
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref currentVelocity, agentSmoothTime);
        return cohesionMove;
    }
}