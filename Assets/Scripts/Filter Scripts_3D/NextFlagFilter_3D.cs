using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "3DFlock/Filter/NextFlagFilter")]
public class NextFlagFilter_3D : ContextFilter_3D
{
    [SerializeField] private float nextFlagDistance = 1f;

    private static int flagCnt = 0;

    public override List<Transform> Filter(FlockAgent_3D agent, List<Transform> original)
    {
        List<Transform> nextFlag = new List<Transform>();
        if (original.Count <= flagCnt) { return nextFlag; }

        nextFlag.Add(original[flagCnt]);
        Debug.Log(flagCnt + ", " + nextFlag[0].name + " : " + nextFlag[0].position);
        //Debug.Log(flagCnt + ", " + nextFlag[0].name + " : " +nextFlag[0].position);
        //Debug.Log(Vector3.Distance(agent.transform.position, nextFlag[0].position));
        if (nextFlagDistance >= Vector3.Distance(agent.transform.position, nextFlag[0].position))
        {
            flagCnt++;
        }
        return nextFlag;
    }
}
