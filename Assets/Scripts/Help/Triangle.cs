using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class Triangle 
{
    //�ﰢ���� ������ A,B,C
    public Point A { get; private set; }
    public Point B { get; private set; }
    public Point C { get; private set; }
    public Point D { get; private set; }
    //������
    public Sphere Circumscriber {  get; private set; }
    
    public Triangle(Point a, Point b, Point c, Point d)
    {
        A = a;
        B = b;
        C = c;
        D = d;

        Circumscriber = CalcCircumcircle();
        
    }


    //���� 3D�� ���ü�� ���ؾ���
    private Sphere CalcCircumcircle()
    {
        //���Ϳ� A,B,C �� �������� ��ǥ�� �־���
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
            planes[i] = new Plane(p1, p2, p3); //������ ���� ���������� �ϴ���� ����
        }

        //������ ã��
        Vector3 intersection = Vector3.zero;

        foreach (Plane plane in planes)
        {
            intersection += plane.normal * plane.distance;
        }//�븻���� * ����� �Ÿ�
        Vector3 center = intersection / 4f;

        float radius = Vector3.Distance(center, A);



        return new Sphere(center, radius);


    }
    public bool ContainsVertex(Point p)
    {
        return A == p || B == p || C == p || D == p;
    }
}
