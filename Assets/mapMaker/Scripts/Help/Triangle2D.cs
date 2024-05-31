using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle2D
{
    public Point A { get; set; }
    public Point B { get; set; }

    public Point C { get; set; }

    public Circumscriber2D Circumscriber { get; set;}

    public Triangle2D(Point u, Point v, Point w)
    {
        A = u; B = v; C = w;

        Circumscriber = CalcCircumcircle();
    }

    private Circumscriber2D CalcCircumcircle()
    {
        Vector3 a = A.Position;
        Vector3 b = B.Position;
        Vector3 c = C.Position;

        float ab = a.sqrMagnitude;
        float cd = b.sqrMagnitude;
        float ef = c.sqrMagnitude;

        float circumX = (ab * (c.z - b.z) + cd * (a.z - c.z) + ef * (b.z - a.z)) / (a.x * (c.z - b.z) + b.x * (a.z - c.z) + c.x * (b.z - a.z));
        float circumZ = (ab * (c.x - b.x) + cd * (a.x - c.x) + ef * (b.x - a.x)) / (a.z * (c.x - b.x) + b.z * (a.x - c.x) + c.z * (b.x - a.x));

        Vector3 circum = new Vector3(circumX / 2, a.y,circumZ / 2);
        float circumRadius = Vector3.SqrMagnitude(a - circum);
        
        
        Vector3 center = circum;

        return new Circumscriber2D(center, circumRadius);
    }

    public bool ContainsVertex(Point p)
    {
        return A == p || B == p || C == p;
    }
}
