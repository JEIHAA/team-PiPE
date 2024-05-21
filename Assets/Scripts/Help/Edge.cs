using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Edge 
{
    public Point StartDot { get; private set; }
    public Point EndDot { get; private set;}

    public Edge(Point start, Point end)
    {
        StartDot = start;
        EndDot = end;
    }

    public override bool Equals(object obj)
    {
        if(obj == null || GetType() != obj.GetType()) return false;

        Edge edge = (Edge)obj;
        return (StartDot == edge.StartDot && EndDot == edge.EndDot) || (StartDot == edge.EndDot && EndDot == edge.StartDot);
    
    }

    public override int GetHashCode()
    {
        return StartDot.GetHashCode() ^ EndDot.GetHashCode();
    }
}
