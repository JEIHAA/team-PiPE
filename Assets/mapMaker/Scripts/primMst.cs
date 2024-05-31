using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class primMst
{
    public static List<Edge> MST(List<Edge> edges ,  List<Point> PointList)
    {
        
        List<Edge> result = new List<Edge>();

        //���� ���ִ� ����Ʈ�� ���Ѵ�.
        HashSet<Point> includePoints = new HashSet<Point>();


        //������ ���� ���� ����
        includePoints.Add(PointList[0]);


        //�켱���� ť�� �̿��ؼ� �Ÿ��� ���� ����� ����Ʈ�� ������ result�� �ִ� ����� �ݺ��Ұ���
        PriorityQueue<Edge> edgeQueue = new PriorityQueue<Edge>();

        //�ʱ� ���� �߰�
        foreach(Edge e in edges.Where(e => e.U == PointList[0] || e.V == PointList[0]) )
        {
            

            edgeQueue.Enqueue(e, Vector3.Distance(e.U.Position, e.V.Position));
        }

        while(includePoints.Count < PointList.Count && edgeQueue.Count > 0) {
        
            //�켱���� ť������ ���� �տ��ִ� ������ dequeue
            Edge e = edgeQueue.Dequeue();

            if (includePoints.Contains(e.U) && includePoints.Contains(e.V))
            { continue; }
            result.Add(e);
            Point newPoint = includePoints.Contains(e.U) ? e.V : e.U;
            includePoints.Add(newPoint);

            foreach(var nextE in edges.Where(e => e.U == newPoint || e.V == newPoint))
            {
                if(!includePoints.Contains(nextE.U) || !includePoints.Contains(nextE.V)){
                    edgeQueue.Enqueue(nextE, Vector3.Distance(nextE.U.Position, nextE.V.Position));
                }

            }
        }


        return result;
    }



}

