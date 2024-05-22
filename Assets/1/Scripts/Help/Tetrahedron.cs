using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UIElements;

public class Tetrahedron 
{
    //삼각형의 꼭짓점 A,B,C
    public Point A { get; private set; }
    public Point B { get; private set; }
    public Point C { get; private set; }
    public Point D { get; private set; }
    //외접원
    public Sphere Circumscriber {  get; private set; }
    
    public Tetrahedron(Point a, Point b, Point c, Point d)
    {
       
        A = a;
        B = b;
        C = c;
        D = d;

        Circumscriber = CalcCircumcircle();
        
    }


    //현재 3D로 사면체를 구해야함
    private Sphere CalcCircumcircle()
    {
        float a = new Matrix4x4(
                new Vector4(A.Position.x, B.Position.x, C.Position.x, D.Position.x),
                new Vector4(A.Position.y, B.Position.y, C.Position.y, D.Position.y),
                new Vector4(A.Position.z, B.Position.z, C.Position.z, D.Position.z),
                new Vector4(1, 1, 1, 1)
            ).determinant;

        float aPosSqr = A.Position.sqrMagnitude;
        float bPosSqr = B.Position.sqrMagnitude;
        float cPosSqr = C.Position.sqrMagnitude;
        float dPosSqr = D.Position.sqrMagnitude;

        float Dx = new Matrix4x4(
            new Vector4(aPosSqr, bPosSqr, cPosSqr, dPosSqr),
            new Vector4(A.Position.y, B.Position.y, C.Position.y, D.Position.y),
            new Vector4(A.Position.z, B.Position.z, C.Position.z, D.Position.z),
            new Vector4(1, 1, 1, 1)
        ).determinant;

        float Dy = -(new Matrix4x4(
            new Vector4(aPosSqr, bPosSqr, cPosSqr, dPosSqr),
            new Vector4(A.Position.x, B.Position.x, C.Position.x, D.Position.x),
            new Vector4(A.Position.z, B.Position.z, C.Position.z, D.Position.z),
            new Vector4(1, 1, 1, 1)
        ).determinant);

        float Dz = new Matrix4x4(
            new Vector4(aPosSqr, bPosSqr, cPosSqr, dPosSqr),
            new Vector4(A.Position.x, B.Position.x, C.Position.x, D.Position.x),
            new Vector4(A.Position.y, B.Position.y, C.Position.y, D.Position.y),
            new Vector4(1, 1, 1, 1)
        ).determinant;

        float c = new Matrix4x4(
            new Vector4(aPosSqr, bPosSqr, cPosSqr, dPosSqr),
            new Vector4(A.Position.x, B.Position.x, C.Position.x, D.Position.x),
            new Vector4(A.Position.y, B.Position.y, C.Position.y, D.Position.y),
            new Vector4(A.Position.z, B.Position.z, C.Position.z, D.Position.z)
        ).determinant;

        Vector3 center = new Vector3(
            Dx / (2 * a),
            Dy / (2 * a),
            Dz / (2 * a)
        );

        float radius = Mathf.Sqrt(((Dx * Dx) + (Dy * Dy) + (Dz * Dz) - (4 * a * c)) / (4 * a * a));
        Debug.Log(Dx +" " + Dy + " " + Dz +" " + a + " " + c +" radius");
        Debug.Log(Vector3.Distance(center,A.Position));
        return new Sphere(center, radius);


    }
    public bool ContainsVertex(Point p)
    {
        return A == p || B == p || C == p || D == p;
    }
}
