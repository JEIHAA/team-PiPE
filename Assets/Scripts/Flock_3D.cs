using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class Flock_3D : MonoBehaviour
{
    [SerializeField] private List<Transform> flags;
    public List<Transform> Flags { get { return flags; } }
    [SerializeField] private FlockAgent_3D agentPrefab;
    [SerializeField] private FlockBehavior_3D behavior;
    [SerializeField, Range(1, 500)] private int startingCount = 30;

    private List<FlockAgent_3D> agents = new List<FlockAgent_3D>();

    [SerializeField, Range(1f, 100f)] private float driveFactor = 10f; //움직임 속도 오프셋 계수
    [SerializeField, Range(1f, 100f)] private float maxSpeed = 5f;
    [SerializeField, Range(1f, 100f)] private float neighborRadius = 1.5f; //이웃 범위
    [SerializeField, Range(1f, 100f)] private float avoidanceRadiusMultiplier = 0.1f; //회피 범위 조절 계수
    [SerializeField, Range(1f, 100f)] private float detectionFlagRadius = 5f; // 탐지 범위
    [SerializeField, Range(1f, 100f)] private float followRadiusMultiplier = 0.1f; //플래그 따라가기 범위 조절 계수
    private const float AgentDensity = 0.02f; //군집 밀도

    private float squareMaxSpeed;
    private float squareNeighborRadius;
    private float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius;  } }
    private float squareDetectionRadius;
    private float squareFollowRadius;
    public float SquareFollowRadius { get { return squareFollowRadius; } }

    private void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        squareDetectionRadius = detectionFlagRadius * detectionFlagRadius;
        squareFollowRadius = squareDetectionRadius * followRadiusMultiplier * followRadiusMultiplier;


        for (int i = 0; i < startingCount; i++) {
            Vector3 spawn = Random.insideUnitSphere * startingCount * AgentDensity;
            spawn.y = 0.2f;
            FlockAgent_3D newAgent = Instantiate(agentPrefab, // startingCount만큼 오브젝트 생성, 원 안의 랜덤한 위치에 랜덤한 방향을 바라보게 생성
                spawn, Quaternion.Euler(Vector3.forward), transform);
                // insideUnitCircle: 순수하게 랜덤으로 원 안에 생성할 경우 원의 중앙에 분포도가 높기 때문에 면적 비례로 흩뿌림
                //Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), transform);
            newAgent.name = "Agent " + i;
            newAgent.Init(this);
            agents.Add(newAgent);
        }
    }

    private void Update()
    {
        foreach (FlockAgent_3D agent in agents) {
            List<Transform> context = GetNearbyObjects(agent);

            //FOR DEMO ONLY
            //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            Vector3 move = behavior.CalculateMove(agent, context, this, flags); // Agent 주변의 이웃과 상호작용을 고려하여 움직임
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent_3D agent) // Agent 주변의 이웃들을 찾음
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        // OverlapSphere : 가상의 구의 영역에서 해당 레이어 마스크로 지정된 모든 게임오브젝트의 콜라이더를 배열로 저장
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider) {
                context.Add(c.transform);
            }
        }
        return context;
    }

}
