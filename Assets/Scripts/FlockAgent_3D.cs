using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponent: �ش� ��ũ��Ʈ�� ���Ե� GameObject�� RequireComponent�� ����� ������Ʈ�� �ڵ����� �߰��ǰų� ��õ� ������Ʈ�� �����ϴ��� �˻���
[RequireComponent(typeof(Collider))]
public class FlockAgent_3D : MonoBehaviour
{
    private Flock_3D agentFlock;
    public Flock_3D AgentFlock { get { return agentFlock; } }

    private Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }


    public Vector3 vectorToVisualize = Vector3.zero; // ���� ����
    public Color vectorColor = Color.red; // ������ ����

    private void Start()
    {
        agentCollider = GetComponent<Collider>();
    }

    public void Init(Flock_3D flock)
    {
        agentFlock = flock;
        vectorToVisualize = new Vector3(flock.SquareFollowRadius, flock.SquareFollowRadius, flock.SquareFollowRadius);
    }

    public void Move(Vector3 _velocity) {
        transform.forward = _velocity;
        transform.position += _velocity * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        // �������� ������ �����Ͽ� ���͸� �ð�ȭ
        Vector3 startPoint = transform.position;
        Vector3 endPoint = startPoint + vectorToVisualize;

        // Gizmos�� ����Ͽ� ���͸� �׸���
        Gizmos.color = vectorColor;
        Gizmos.DrawLine(startPoint, endPoint);
    }
}
