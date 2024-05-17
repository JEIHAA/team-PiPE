using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="3DFlock/Filter/PhysicsLayerFilter")]
public class PhysicsLayerFilter_3D : ContextFilter_3D
{
    public LayerMask mask;

    public override List<Transform> Filter(FlockAgent_3D agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        foreach (Transform item in original)
        {
            Debug.Log(item.name);
            if (mask == (mask | (1 << item.gameObject.layer)))
            {
                filtered.Add(item);
            }
            
        }
        return filtered;
    }
}
