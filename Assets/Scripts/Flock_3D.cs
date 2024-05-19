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

    [SerializeField, Range(1f, 100f)] private float driveFactor = 10f; //������ �ӵ� ������ ���
    [SerializeField, Range(1f, 100f)] private float maxSpeed = 5f;
    [SerializeField, Range(1f, 100f)] private float neighborRadius = 1.5f; //�̿� ����
    [SerializeField, Range(1f, 100f)] private float avoidanceRadiusMultiplier = 0.1f; //ȸ�� ���� ���� ���
    [SerializeField, Range(1f, 100f)] private float detectionFlagRadius = 5f; // Ž�� ����
    [SerializeField, Range(1f, 100f)] private float followRadiusMultiplier = 0.1f; //�÷��� ���󰡱� ���� ���� ���
    private const float AgentDensity = 0.02f; //���� �е�

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
            FlockAgent_3D newAgent = Instantiate(agentPrefab, // startingCount��ŭ ������Ʈ ����, �� ���� ������ ��ġ�� ������ ������ �ٶ󺸰� ����
                spawn, Quaternion.Euler(Vector3.forward), transform);
                // insideUnitCircle: �����ϰ� �������� �� �ȿ� ������ ��� ���� �߾ӿ� �������� ���� ������ ���� ��ʷ� ��Ѹ�
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

            Vector3 move = behavior.CalculateMove(agent, context, this, flags); // Agent �ֺ��� �̿��� ��ȣ�ۿ��� ����Ͽ� ������
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent_3D agent) // Agent �ֺ��� �̿����� ã��
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        // OverlapSphere : ������ ���� �������� �ش� ���̾� ����ũ�� ������ ��� ���ӿ�����Ʈ�� �ݶ��̴��� �迭�� ����
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider) {
                context.Add(c.transform);
            }
        }
        return context;
    }

}
