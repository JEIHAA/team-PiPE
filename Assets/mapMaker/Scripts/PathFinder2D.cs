using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PathFinder2D
{
    public class Node
    {
        public Point Position { get; private set; }
        //이전 노드
        public Node Previous { get; set; }

        public float Cost { get; set; }

        public Node(int x, int z)
        {
            Position = new Point(new Vector3(x, 0,z));
        }
    }

    public struct PathCost
    {
        public bool traversable;
        public float cost;
    }

    static readonly Vector3[] neighbors =
    {
        new Vector3(2, 0, 0),
        new Vector3(-2, 0, 0),
        new Vector3(0, 0, 2),
        new Vector3(0, 0, -2)
    };

    Grid2D<Node> grid;
    PriorityQueue<Node> queue;
    HashSet<Node> closed;
    Stack<Vector3> stack;
    int sizex;
    int sizez;
    

    public PathFinder2D(Vector3 size)
    {
        grid = new Grid2D<Node>(size, Vector3.zero);

        sizex = (int)size.x;
        sizez = (int)size.z;        

        queue = new PriorityQueue<Node>();
        closed = new HashSet<Node>();
        stack = new Stack<Vector3>();

        for(int x = 0; x<sizex; x++)
        {
            for(int z = 0; z < sizez; z++)
            {
                grid[x, size.y, z] = new Node(x, z);
            }
        }
    }

    void ResetNodes()
    {
        var size = grid.Size;
        for(int x = 0; x<size.x; x++)
        {
            for(int z = 0; z<size.z; z++)
            {
                var node = grid[x, size.y, z];
                node.Previous = null;
                node.Cost = float.PositiveInfinity;
            }
        }
    }

    public List<Vector3> FindPath(Point start, Point end, Func<Node, Node, PathCost> costFunction)
    {
        ResetNodes();
        for(int i=0; i<queue.Count; i++)
        {
            queue.Dequeue();
        }
        closed.Clear();

        queue = new PriorityQueue<Node>();
        closed = new HashSet<Node>();


        grid[start.Position].Cost = 0;
        queue.Enqueue(grid[start.Position], 0);
        

        while(queue.Count > 0)
        {
            Node node = queue.DDequeue();
            closed.Add(node);
            
            

            if(node.Position.Position == end.Position)
            {
                return ReconstructPath(node);
            }
            foreach(var offset in neighbors)
            {
                
                
                if (!grid.InBounds(node.Position.Position + offset)) {  continue; }

                var neighbor = grid[node.Position.Position + offset];
                if (closed.Contains(neighbor)) {  continue; }

                var pathCost = costFunction(node, neighbor);
                if (!pathCost.traversable) {  continue; }
                
                float newCost = node.Cost + pathCost.cost;
                
                if (newCost < neighbor.Cost)
                {
                    
                    neighbor.Previous = node;
                    neighbor.Cost = newCost;
                    
                    

                    if (queue.TryGetPriority(node, node.Cost))
                    {

                        queue.UpdatePriority(node, newCost);
                    }
                    else
                    {

                        queue.Enqueue(neighbor, neighbor.Cost);
                    }
                }
                
            }
        }
        return null;
    }

    List<Vector3> ReconstructPath(Node node)
    {
        List<Vector3> result = new List<Vector3>();

        while (node != null)
        {
            
            stack.Push(node.Position.Position);
            node = node.Previous;
            
        }

        while (stack.Count > 0)
        {
            
            result.Add(stack.Pop());
        }

        return result;
    }


}
