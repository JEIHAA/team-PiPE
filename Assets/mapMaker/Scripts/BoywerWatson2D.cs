using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoywerWatson2D
{
    public List<Edge> Edges { get; private set; }
    public List<Triangle2D> Triangles { get; private set; }
    public List<Point> pointList { get; private set; }

    private BoywerWatson2D()
    {
        Edges = new List<Edge>();
        Triangles = new List<Triangle2D>();
    }

    public static BoywerWatson2D Triangulate(List<Room> rooms)
    {
        BoywerWatson2D delaunay = new BoywerWatson2D();
        List<Point> PointList = new List<Point>();
        foreach (Room room in rooms)
        {
            PointList.Add(room.doorPoint);
        }

        delaunay.pointList = PointList;
        delaunay.setTriangles();


        return delaunay;
    }

    private static Triangle2D CreateSuperTriangle(List<Point> points)
    {
        float minX = points[0].Position.x;
        float minZ = points[0].Position.z;
        float maxX = points[0].Position.x;
        float maxZ = points[0].Position.z;
        float Y = points[0].Position.y;
        foreach (var point in points)
        {
            minX = Mathf.Min(minX, point.Position.x);
            minZ = Mathf.Min(minZ, point.Position.z);
            maxX = Mathf.Max(maxX, point.Position.x);
            maxZ = Mathf.Max(maxZ, point.Position.z);

        }

        float dx = maxX - minX;
        float dy = maxZ - minZ;
        float deltaMax = Mathf.Max(dx, dy) * 2;

        Point p1 = new Point(new Vector3(minX - 1, Y,minZ - 1));
        Point p2 = new Point(new Vector3(minX - 1, Y,maxZ + deltaMax));
        Point p3 = new Point(new Vector3(maxX + deltaMax, Y,minZ - 1));


        return new Triangle2D(p1, p2, p3);

    }



    private void setTriangles()
    {
        List<Triangle2D> triangulation = new List<Triangle2D>();

        HashSet<Edge> edgeSet = new HashSet<Edge>();

        Triangle2D superTriangle = CreateSuperTriangle(pointList);



        triangulation.Add(superTriangle); //���� ū ���ü�� �߰�

        foreach (var point in pointList) // ex)����Ʈ 11���� �������� �˻�
        {
            List<Triangle2D> badtriangle = new List<Triangle2D>(); // badTriangle�˻� 
            foreach (Triangle2D tri in triangulation)// ����Ʈ���� �ﰢ�� �˻�
            {
                if (tri.Circumscriber.Contains(point)) // �ش� �ﰢ���� �������� ���� �����ϰ��ִ°�?
                {

                    badtriangle.Add(tri); // �������ȿ� ��ǥ�� ������ bad Triangle�� �־��ش�.
                }

            }

            List<Edge> polygon = new List<Edge>();


            foreach (var tri in badtriangle)
            {
                foreach(Edge edge in GetEdges(tri))
                {
                    if(!IsShared(edge, badtriangle))
                    {
                        polygon.Add(edge);
                    }
                }
            }
            foreach(Triangle2D tri in badtriangle)
            {
                triangulation.Remove(tri);
            }

            foreach (Edge edge in polygon)
            {
                Triangle2D newTriangle = new Triangle2D(edge.U, edge.V, point);
                triangulation.Add(newTriangle);
            }

        }

        triangulation.RemoveAll(tri =>
        tri.ContainsVertex(superTriangle.A) ||
        tri.ContainsVertex(superTriangle.B) ||
        tri.ContainsVertex(superTriangle.C));



        Triangles = triangulation;


        //���� Tetrahedrons�� ������Ҹ� �̿��Ͽ�, �ﰢ���� edge�� ��������

        foreach (Triangle2D t in triangulation)
        {
            

            Edge ab = new Edge(t.A, t.B);
            Edge bc = new Edge(t.B, t.C);
            Edge ca = new Edge(t.C, t.A);

            if (edgeSet.Add(ab))
            {
                Edges.Add(ab);
            }
            if (edgeSet.Add(bc))
            {
                Edges.Add(bc);
            }
            if (edgeSet.Add(ca))
            {
                Edges.Add(ca);
            }
        }


    }
    private static List<Edge> GetEdges(Triangle2D triangle)
    {
        return new List<Edge>
        {
            new Edge(triangle.A, triangle.B),
            new Edge(triangle.B, triangle.C),
            new Edge(triangle.C, triangle.A)
        }; 
    }
    private static bool IsShared(Edge edge, List<Triangle2D> triangles)
    {
        int count = 0;

        foreach (var triangle in triangles)
        {
            foreach (var triEdge in GetEdges(triangle))
            {
                if (edge.Equals(triEdge))
                {
                    count++;
                }
            }
        }

        return count > 1;
    }


}
