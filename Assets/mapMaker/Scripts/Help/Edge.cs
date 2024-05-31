using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Edge 
{
    public Point U { get; private set; }
    public Point V { get; private set;}

    public Edge(Point start, Point end)
    {
        U = start;
        V = end;
    }

    public static bool operator ==(Edge left, Edge right)
    {
        return (left.U == right.U || left.U == right.V)
            && (left.V == right.U || left.V == right.V);
    }

    public static bool operator !=(Edge left, Edge right)
    {
        return !(left == right);
    }

    public override bool Equals(object obj)
    {
        if (obj is Edge e)
        {
            return this == e;
        }

        return false;
    }

    public bool Equals(Edge e)
    {
        return this == e;
    }

    public override int GetHashCode()
    {
        return U.GetHashCode() ^ V.GetHashCode();
    }
}
