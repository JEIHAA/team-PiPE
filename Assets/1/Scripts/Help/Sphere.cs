using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere
{
    public Vector3 Center { get; private set; }
    public float Radius { get; private set;}

    public Sphere(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }


    
    public bool Contains(Point p)
    {
        
        //만약 point에서 center의 거리가 radius보다 작거나 같으면 통과
        return Vector3.Distance(p.Position, Center) <= Radius;
    }
}
