using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private bool x, y, z;
    [SerializeField] private Transform target;

    public void Update()
    {
        if (!target) return;

        transform.position = new Vector3(       
            (x ? target.position.x : target.position.x),
            (y ? target.position.y : target.position.y),
            (z ? target.position.z : target.position.z));         
    }
}
