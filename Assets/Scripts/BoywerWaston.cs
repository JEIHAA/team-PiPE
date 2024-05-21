using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoywerWaston 
{
    public static List<Triangle> Triangles(List<Point> pointList)
    {
        List<Triangle> triangulation = new List<Triangle>();

        

        Triangle superTriangle = CreateSuperTriangle(pointList);

        

        triangulation.Add(superTriangle); //제일 큰 사면체를 추가

        foreach (var point in pointList) // ex)포인트 11개를 돌때까지 검사
        {
            List<Triangle> badtriangle = new List<Triangle>(); // badTriangle검사 
            foreach (Triangle tri in triangulation)// 리스트내의 삼각형 검사
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
                
                Triangle newTri = new Triangle(face.A, face.B, face.C, point);
                Debug.Log(newTri.A.Position + " " + newTri.B.Position + " " + newTri.C.Position + " " + newTri.D.Position);
                
                triangulation.Add(newTri);
            }

        }

        triangulation.RemoveAll(tri =>
        tri.ContainsVertex(superTriangle.A) ||
        tri.ContainsVertex(superTriangle.B) ||
        tri.ContainsVertex(superTriangle.C) ||
        tri.ContainsVertex(superTriangle.D));



        return triangulation;
    }

    private static Triangle CreateSuperTriangle(List<Point> points)
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float minZ = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;
        float maxZ = float.MinValue;

        foreach (var point in points)
        {
            if (point.Position.x < minX) minX = point.Position.x;
            if (point.Position.y < minY) minY = point.Position.y;
            if (point.Position.z < minZ) minZ = point.Position.z;
            if (point.Position.x > maxX) maxX = point.Position.x;
            if (point.Position.y > maxY) maxY = point.Position.y;
            if (point.Position.z > maxZ) maxZ = point.Position.z;
        }

        float dx = maxX - minX;
        float dy = maxY - minY;
        float dz = maxZ - minZ;
        float deltaMax = Mathf.Max(dx, dy, dz);
        Vector3 center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, (minZ + maxZ) / 2f);

        Point p1 = new Point(center + new Vector3(-20 * deltaMax, -20 * deltaMax, -20 * deltaMax));
        Point p2 = new Point(center + new Vector3(20 * deltaMax, 0, 0));
        Point p3 = new Point(center + new Vector3(0, 20 * deltaMax, 0));
        Point p4 = new Point(center + new Vector3(0, 0, 20 * deltaMax));

        return new Triangle(p1, p2, p3, p4);

    }
    private static List<Face> GetFaces(Triangle tetra)
    {
        return new List<Face>
        {
            new Face(tetra.A, tetra.B, tetra.C),
            new Face(tetra.A, tetra.B, tetra.D),
            new Face(tetra.A, tetra.C, tetra.D),
            new Face(tetra.B, tetra.C, tetra.D)
        };
    }

    private static bool IsShared(Face face, List<Triangle> tetrahedra)
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
}
