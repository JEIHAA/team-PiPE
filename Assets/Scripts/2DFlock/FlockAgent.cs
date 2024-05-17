using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponent: �ش� ��ũ��Ʈ�� ���Ե� GameObject�� RequireComponent�� ����� ������Ʈ�� �ڵ����� �߰��ǰų� ��õ� ������Ʈ�� �����ϴ��� �˻���
[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    private void Start()
    {
        agentCollider = GetComponent<Collider2D>();
    }

    public void Init(Flock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector2 _velocity) {
        transform.up = _velocity;
        transform.position += (Vector3)_velocity * Time.deltaTime;
    }
}
