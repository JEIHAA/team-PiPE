using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponent: 해당 스크립트가 포함된 GameObject에 RequireComponent에 명시한 컴포넌트가 자동으로 추가되거나 명시된 컴포넌트가 존재하는지 검사함
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
