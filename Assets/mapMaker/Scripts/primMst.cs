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

        //현재 들어가있는 포인트를 말한다.
        HashSet<Point> includePoints = new HashSet<Point>();


        //임의의 시작 정점 선택
        includePoints.Add(PointList[0]);


        //우선순위 큐를 이용해서 거리상 제일 가까운 포인트를 꺼내서 result에 넣는 방식을 반복할거임
        PriorityQueue<Edge> edgeQueue = new PriorityQueue<Edge>();

        //초기 간선 추가
        foreach(Edge e in edges.Where(e => e.U == PointList[0] || e.V == PointList[0]) )
        {
            

            edgeQueue.Enqueue(e, Vector3.Distance(e.U.Position, e.V.Position));
        }

        while(includePoints.Count < PointList.Count && edgeQueue.Count > 0) {
        
            //우선순위 큐로인해 가장 앞에있는 간선을 dequeue
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

