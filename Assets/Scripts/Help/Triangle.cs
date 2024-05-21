using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class Triangle 
{
    //삼각형의 꼭짓점 A,B,C
    public Point A { get; private set; }
    public Point B { get; private set; }
    public Point C { get; private set; }
    public Point D { get; private set; }
    //외접원
    public Sphere Circumscriber {  get; private set; }
    
    public Triangle(Point a, Point b, Point c, Point d)
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
        //벡터에 A,B,C 각 꼭짓점의 좌표를 넣어줌
        Vector3 A = this.A.Position;
        Vector3 B = this.B.Position;
        Vector3 C = this.C.Position;
        Vector3 D = this.D.Position;

        Vector3[] vertices = new Vector3[] { A, B, C, D };

        Plane[] planes = new Plane[4];

        for (int i = 0; i < 4; i++)
        {
            Vector3 p1 = vertices[(i + 1) % 4];
            Vector3 p2 = vertices[(i + 2) % 4];
            Vector3 p3 = vertices[(i + 3) % 4];
            planes[i] = new Plane(p1, p2, p3); //세개의 점을 꼭짓점으로 하는평면 생성
        }

        //교차점 찾기
        Vector3 intersection = Vector3.zero;

        foreach (Plane plane in planes)
        {
            intersection += plane.normal * plane.distance;
        }//노말벡터 * 평면의 거리
        Vector3 center = intersection / 4f;

        float radius = Vector3.Distance(center, A);



        return new Sphere(center, radius);


    }
    public bool ContainsVertex(Point p)
    {
        return A == p || B == p || C == p || D == p;
    }
}
