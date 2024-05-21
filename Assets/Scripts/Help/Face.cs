using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face //다각형의 면 표현 및 계산용
{
   public Point A { get; private set; }
   public Point B { get; private set; }
   public Point C { get; private set; }

    public Face(Point a, Point b, Point c)
    {
        A = a; B = b; C = c;
    }

    public override bool Equals(object obj)
    {
        if(obj == null || GetType()!= obj.GetType()) return false;

        Face face = (Face)obj;
        HashSet<Point> thisPoints = new HashSet<Point> { A, B, C };
        HashSet<Point> otherPoints = new HashSet<Point> { face.A, face.B, face.C };
        return thisPoints.SetEquals(otherPoints);
    }
    public override int GetHashCode()
    {
        return A.GetHashCode() ^ B.GetHashCode() ^ C.GetHashCode();
    }
}
