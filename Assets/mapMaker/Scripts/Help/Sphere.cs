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
        
        //���� point���� center�� �Ÿ��� radius���� �۰ų� ������ ���
        return Vector3.Distance(p.Position, Center) <= Radius;
    }
}
