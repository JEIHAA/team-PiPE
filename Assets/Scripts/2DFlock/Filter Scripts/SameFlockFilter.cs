using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "2DFlock/Filter/SameFlock")]
public class SameFlockFilter : ContextFilter
{
    public override List<Transform> Filter(FlockAgent agent, List<Transform> original) { 
        List<Transform> filtered = new List<Transform>();
        foreach (Transform item in original) {
            FlockAgent itemAgent = item.GetComponent<FlockAgent>();
            if (itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock) { // 무리가 존재하는지, 해당 개체가 자신의 무리인지 검사
                filtered.Add(item);
            }
        }
        return filtered;
    }
}
