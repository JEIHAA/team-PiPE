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
        //����� ��ǥ�� ������ �������� ��ȯ���� ����
        if (context.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 followMove = Vector3.zero;
        int nFollow = 0; // �����ؾ��ϴ� �÷��� ��
        foreach (Transform item in flags)
        {
            Debug.Log(item);
            if (Vector3.SqrMagnitude(item.position - agent.transform.position) <flock.SquareFollowRadius)
            { // �̿� ��ü���� �Ÿ��� ���� �ݰ溸�� ���� ���
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
