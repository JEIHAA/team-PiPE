using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circumscriber2D
{
    public Vector3 Center { get; private set; }
    public float Radius { get; private set; }

    public Circumscriber2D(Vector3 center, float radius)
    {
        Center = center;// CIRCUM
        Radius = radius; // Vector3.sQR(A - CIRCUM)
    }

    public bool Contains(Point p)
    {
        return Vector3.SqrMagnitude(p.Position - Center) <= Radius;
    }

}
