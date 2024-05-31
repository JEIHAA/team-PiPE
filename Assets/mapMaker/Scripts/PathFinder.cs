using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder 
{
    public class Node
    {
        public Vector3 Position { get; private set; }
        public Node Previous { get; private set; }
        public HashSet<Vector3> PreviousSet { get; private set; }
        public float Cost { get; private set; }

        public Node(Vector3 position)
        {
            Position = position;
            PreviousSet = new HashSet<Vector3>();
        }
    }

    public struct PathCost
    {
        public bool traversable;
        public float cost;
        public bool isStairs;
    }
    //이웃한 블럭의 좌표
    //통로, stair정도로만 생각하면될듯?
    static readonly Vector3[] neighbors = {
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),

        new Vector3(3, 1, 0),
        new Vector3(-3, 1, 0),
        new Vector3(0, 1, 3),
        new Vector3(0, 1, -3),

        new Vector3(3, -1, 0),
        new Vector3(-3, -1, 0),
        new Vector3(0, -1, 3),
        new Vector3(0, -1, -3),
    };
    PriorityQueue<Node> queue;
    HashSet<Node> closed;
    Stack<Vector3> stack;
    
    public PathFinder(Vector3 size)
    {
        queue = new PriorityQueue<Node>();
        closed = new HashSet<Node>();
        stack = new Stack<Vector3>();

        for(int x = 0; x< size.x; x++)
        {
            for(int y = 0; y< size.y; y++)
            {
                for(int z = 0; z< size.z;)
                {

                }
            }
        }
    }

}
