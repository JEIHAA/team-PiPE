using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Point
{
    public Vector3 Position { get; private set; }

    public Point(Vector3 position)
    {
        Position = position;
    }
    
}
