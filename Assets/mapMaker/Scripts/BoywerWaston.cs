using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoywerWaston 
{
    public List<Edge> Edges { get; private set; }
    public List<Tetrahedron> Tetrahedrons { get; private set; }

    public List<Triangle> Triangles { get; private set; }
    public List<Point> pointList { get; private set; }
    
    private BoywerWaston()
    {
        Edges = new List<Edge>();
        Triangles = new List<Triangle>();
        Tetrahedrons = new List<Tetrahedron>();
    }

    public static BoywerWaston Triangulate(List<Room> rooms)
    {
        BoywerWaston delaunay = new BoywerWaston();
        List<Point> PointList = new List<Point>();
        foreach(Room room in rooms)
        {
            PointList.Add(room.point);
        }

        delaunay.pointList = PointList;
        delaunay.setTetrahedron();


        return delaunay;
    }

    private static Tetrahedron CreateSuperTriangle(List<Point> points)
    {
        float minX = points[0].Position.x;
        float minY = points[0].Position.y;
        float minZ = points[0].Position.z;
        float maxX = points[0].Position.x;
        float maxY = points[0].Position.y;
        float maxZ = points[0].Position.z;

        foreach (var point in points)
        {
            minX = Mathf.Min(minX, point.Position.x);
            minY = Mathf.Min(minY, point.Position.y);
            minZ = Mathf.Min(minZ, point.Position.z);
            maxX = Mathf.Max(maxX, point.Position.x);
            maxY = Mathf.Max(maxY, point.Position.y);
            maxZ = Mathf.Max(maxZ, point.Position.z);
            
        }

        float dx = maxX - minX;
        float dy = maxY - minY;
        float dz = maxZ - minZ;
        float deltaMax = Mathf.Max(dx, dy, dz)*2;

        Point p1 = new Point(new Vector3(minX - 1, minY - 1, minZ - 1));
        Point p2 = new Point(new Vector3(maxX + deltaMax, minY - 1, minZ - 1));
        Point p3 = new Point(new Vector3(minX - 1, maxY + deltaMax, minZ - 1));
        Point p4 = new Point(new Vector3(minX - 1, minY - 1, maxZ + deltaMax));

        return new Tetrahedron(p1, p2, p3, p4);

    }
    private static List<Face> GetFaces(Tetrahedron tetra)
    {
        return new List<Face>
        {
            new Face(tetra.A, tetra.B, tetra.C),
            new Face(tetra.A, tetra.B, tetra.D),
            new Face(tetra.A, tetra.C, tetra.D),
            new Face(tetra.B, tetra.C, tetra.D)
        };
    }

    private static bool IsShared(Face face, List<Tetrahedron> tetrahedra)
    {
        //리스트의 badtriangle과 face 리스트를 비교

        int count = 0;

        
        foreach (var tetra in tetrahedra)
        {
            foreach (var tetraFace in GetFaces(tetra))
            {
                if (face.Equals(tetraFace))
                {
                    count++;
                }
            }
        }

        return count > 1;
    }

    private void setTetrahedron()
    {
        List<Tetrahedron> triangulation = new List<Tetrahedron>();

        HashSet<Triangle> triangleSet = new HashSet<Triangle>();
        HashSet<Edge> edgeSet = new HashSet<Edge>();

        Tetrahedron superTriangle = CreateSuperTriangle(pointList);



        triangulation.Add(superTriangle); //제일 큰 사면체를 추가

        foreach (var point in pointList) // ex)포인트 11개를 돌때까지 검사
        {
            List<Tetrahedron> badtriangle = new List<Tetrahedron>(); // badTriangle검사 
            foreach (Tetrahedron tri in triangulation)// 리스트내의 삼각형 검사
            {
                if (tri.Circumscriber.Contains(point)) // 해당 삼각형의 외접구는 점을 포함하고있는가?
                {
                    
                    badtriangle.Add(tri); // 외접원안에 좌표가 있으면 bad Triangle에 넣어준다.
                }

            }



            List<Face> polygon = new List<Face>(); // bad triangle을 기반으로 새로운 폴리곤 생성

            foreach (var tri in badtriangle) //배드 트라이앵글안에있는 triangle
            {
                foreach (var face in GetFaces(tri)) // triangle에 face를 가져온다.
                {
                    if (!IsShared(face, badtriangle))
                    {

                        polygon.Add(face);
                    }
                }
            }

            foreach (var tri in badtriangle)
            {
                triangulation.Remove(tri);
            }

            foreach (var face in polygon)
            {

                Tetrahedron newTri = new Tetrahedron(face.A, face.B, face.C, point);
                

                triangulation.Add(newTri);
            }

        }

        triangulation.RemoveAll(tri =>
        tri.ContainsVertex(superTriangle.A) ||
        tri.ContainsVertex(superTriangle.B) ||
        tri.ContainsVertex(superTriangle.C) ||
        tri.ContainsVertex(superTriangle.D));



        Tetrahedrons = triangulation;


        //이제 Tetrahedrons의 구성요소를 이용하여, 삼각형와 edge를 담을거임

        foreach(Tetrahedron t in triangulation)
        {
            Triangle abc = new Triangle(t.A, t.B, t.C);
            Triangle abd = new Triangle(t.A, t.B, t.D);
            Triangle acd = new Triangle(t.A, t.C, t.D);
            Triangle bcd = new Triangle(t.B, t.C, t.D);

            if (triangleSet.Add(abc))
            {
                Triangles.Add(abc);
            }
            if (triangleSet.Add(abd))
            {
                Triangles.Add(abd);
            }
            if (triangleSet.Add(acd))
            {
                Triangles.Add(acd);
            }
            if (triangleSet.Add(bcd))
            {
                Triangles.Add(bcd);
            }

            Edge ab = new Edge(t.A, t.B);
            Edge bc = new Edge(t.B, t.C);
            Edge ca = new Edge(t.C, t.A);
            Edge da = new Edge(t.D, t.A);
            Edge db = new Edge(t.D, t.B);
            Edge dc = new Edge(t.D, t.C);

            if(edgeSet.Add(ab))
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
            if (edgeSet.Add(da))
            {
                Edges.Add(da);
            }
            if (edgeSet.Add(db))
            {
                Edges.Add(db);
            }
            if (edgeSet.Add(dc))
            {
                Edges.Add(dc);
            }
        }
    }
}
