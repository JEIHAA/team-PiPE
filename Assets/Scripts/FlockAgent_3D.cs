using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponent: 해당 스크립트가 포함된 GameObject에 RequireComponent에 명시한 컴포넌트가 자동으로 추가되거나 명시된 컴포넌트가 존재하는지 검사함
[RequireComponent(typeof(Collider))]
public class FlockAgent_3D : MonoBehaviour
{
    private Flock_3D agentFlock;
    public Flock_3D AgentFlock { get { return agentFlock; } }

    private Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }


    public Vector3 vectorToVisualize = Vector3.zero; // 예시 벡터
    public Color vectorColor = Color.red; // 벡터의 색상

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
        // 시작점과 끝점을 설정하여 벡터를 시각화
        Vector3 startPoint = transform.position;
        Vector3 endPoint = startPoint + vectorToVisualize;

        // Gizmos를 사용하여 벡터를 그리기
        Gizmos.color = vectorColor;
        Gizmos.DrawLine(startPoint, endPoint);
    }
}
