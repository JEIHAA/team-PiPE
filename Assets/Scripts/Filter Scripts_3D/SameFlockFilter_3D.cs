using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "3DFlock/Filter/SameFlock")]
public class SameFlockFilter_3D : ContextFilter_3D
{
    public override List<Transform> Filter(FlockAgent_3D agent, List<Transform> original) { 
        List<Transform> filtered = new List<Transform>();
        foreach (Transform item in original) {
            FlockAgent_3D itemAgent = item.GetComponent<FlockAgent_3D>();
            if (itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock) { // ������ �����ϴ���, �ش� ��ü�� �ڽ��� �������� �˻�
                filtered.Add(item);
            }
        }
        return filtered;
    }
}
