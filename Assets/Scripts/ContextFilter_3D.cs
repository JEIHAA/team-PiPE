using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContextFilter_3D : ScriptableObject
{
    public abstract List<Transform> Filter(FlockAgent_3D agent, List<Transform> original);
}
