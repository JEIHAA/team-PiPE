using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DelaunayTriangulation : MonoBehaviour
{
    private Transform[] transforms;

    private List<Vector3> points = new List<Vector3>();

    private List<Room> rooms;
    private BoywerWaston delaunay;

    System.Random random = new System.Random();

    [SerializeField]
    GameObject roadPrefab;

    // using incremental algorithm

    private void Start()
    {
        rooms = new List<Room>(); //방의 크기와 위치 저장하는곳
        transforms = GetComponentsInChildren<Transform>();

        


        List<Point> pointList = new List<Point>();

        foreach (Transform t in transforms)
        {
            if (t != transform && t.gameObject.layer == 7)
            {
                Debug.Log(t);
                
                rooms.Add(new Room(t.transform, t.localScale));
            }

        }




        delaunay = BoywerWaston.Triangulate(rooms); // 삼각분할

        GenerateRoads( PlayMST());




        
    }

    void PlaceRoom() { }




    private HashSet<Edge> PlayMST()
    {
        List<Edge> edges = new List<Edge>();
        edges = delaunay.Edges;
        List<Edge> edges2 = new List<Edge>();

        
        edges2 = primMst.MST(edges, delaunay.pointList);

        
        HashSet<Edge> remainEdges = new HashSet<Edge>(edges);
        HashSet<Edge> seletedEdges = new HashSet<Edge>(edges2);

        remainEdges.ExceptWith(seletedEdges);

        

        foreach(var edge in remainEdges)
        {
            double a = random.NextDouble();
            if(a < 0.125)
            {
                seletedEdges.Add(edge);
            }
        }



        foreach (Edge edge in seletedEdges)
        {
            Debug.DrawLine(edge.U.Position, edge.V.Position, Color.blue, 100f);
        }
        return seletedEdges;
    }

    private void GenerateRoads(HashSet<Edge> edges)
    {
       
        
        foreach (Edge edge in edges)
        {
            Room startRoom = null;
            Room endRoom = null;
            foreach(Room r in rooms)
            {
                if(r.point == edge.U)
                {
                    startRoom = r;
                }
                if(r.point == edge.V)
                {
                    endRoom = r;
                }
            }
            Point startPoint = startRoom.point;
            Point endPoint = endRoom.point;

            

            
        }
 
    }



}
