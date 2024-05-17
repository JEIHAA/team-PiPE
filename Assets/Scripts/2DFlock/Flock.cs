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
    const float AgentDensity = 0.08f; //���� �е�

    [Range (1f, 100f)]
    public float driveFactor = 10f; //������ �ӵ� ������ ���
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 100f)]
    public float neighborRadius = 1.5f; //�̿� ����
    [Range(1f, 100f)]
    public float avoidanceRadiusMultiplier = 1f; //ȸ�� ���� ���� ���

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
            FlockAgent newAgent = Instantiate( agentPrefab, // startingCount��ŭ ������Ʈ ����, �� ���� ������ ��ġ�� ������ ������ �ٶ󺸰� ����
                Random.insideUnitCircle * startingCount * AgentDensity, // insideUnitCircle: �����ϰ� �������� �� �ȿ� ������ ��� ���� �߾ӿ� �������� ���� ������ ���� ��ʷ� ��Ѹ�
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

            Vector2 move = behavior.CalculateMove(agent, context, this); // Agent �ֺ��� �̿��� ��ȣ�ۿ��� ����Ͽ� ������
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent) // Agent �ֺ��� �̿����� ã��
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        // OverlapCircleAll : ������ ���� �������� �ش� ���̾� ����ũ�� ������ ��� ���ӿ�����Ʈ�� �ݶ��̴��� �迭�� ����
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider) {
                context.Add(c.transform);
            }
        }
        return context;
    }

}
