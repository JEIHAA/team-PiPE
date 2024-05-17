using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;

    [Range(10, 500)]
    public int startingCount = 30;
    const float AgentDensity = 0.08f; //군집 밀도

    [Range (1f, 100f)]
    public float driveFactor = 10f; //움직임 속도 오프셋 계수
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 100f)]
    public float neighborRadius = 1.5f; //이웃 범위
    [Range(1f, 100f)]
    public float avoidanceRadiusMultiplier = 1f; //회피 범위 조절 계수

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius;  } }

    private void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;


        for (int i = 0; i < startingCount; i++) { 
            FlockAgent newAgent = Instantiate( agentPrefab, // startingCount만큼 오브젝트 생성, 원 안의 랜덤한 위치에 랜덤한 방향을 바라보게 생성
                Random.insideUnitCircle * startingCount * AgentDensity, // insideUnitCircle: 순수하게 랜덤으로 원 안에 생성할 경우 원의 중앙에 분포도가 높기 때문에 면적 비례로 흩뿌림
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), transform);
            newAgent.name = "Agent " + i;
            newAgent.Init(this);
            agents.Add(newAgent);
        }
    }

    private void Update()
    {
        foreach (FlockAgent agent in agents) {
            List<Transform> context = GetNearbyObjects(agent);

            //FOR DEMO ONLY
            //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            Vector2 move = behavior.CalculateMove(agent, context, this); // Agent 주변의 이웃과 상호작용을 고려하여 움직임
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent) // Agent 주변의 이웃들을 찾음
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        // OverlapCircleAll : 생성된 원의 영역에서 해당 레이어 마스크로 지정된 모든 게임오브젝트의 콜라이더를 배열로 저장
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider) {
                context.Add(c.transform);
            }
        }
        return context;
    }

}
