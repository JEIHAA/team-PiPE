using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle 
{
    public Point U { get; set; }
    public Point V { get; set; }

    public Point W { get; set; }

    public bool IsBad { get; set; }

    public Triangle() { }

    public Triangle(Point u, Point v, Point w)
    {
        U = u; V = v; W = w;
    }

    public static bool operator ==(Triangle left, Triangle right)
    {
        return (left.U == right.U || left.U == right.V || left.U == right.W)
            && (left.V == right.U || left.V == right.V || left.V == right.W)
            && (left.W == right.U || left.W == right.V || left.W == right.W);
    }

    public static bool operator !=(Triangle left, Triangle right)
    {
        return !(left == right);
    }

    public override bool Equals(object obj)
    {
        if (obj is Triangle e)
        {
            return this == e;
        }

        return false;
    }

    public bool Equals(Triangle e)
    {
        return this == e;
    }

    public override int GetHashCode()
    {
        return U.GetHashCode() ^ V.GetHashCode() ^ W.GetHashCode();
    }


}
