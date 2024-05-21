using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelaunayTriangulation : MonoBehaviour
{
    private Transform[] transforms;

    private List<Vector3> points = new List<Vector3>();

    // using incremental algorithm

    private void Start()
    {
        transforms = GetComponentsInChildren<Transform>();

        List<Point> pointList = new List<Point>();

        foreach (Transform t in transforms)
        {
            if (t != transform)
            {
                points.Add(t.position);
            }

        }

        foreach(Vector3 point in points)
        {
            pointList.Add(new Point(point));
        }


        List<Triangle> triangulation = BoywerWaston.Triangles(pointList);


        foreach (var tetra in triangulation)
        {
            Debug.DrawLine(tetra.A.Position, tetra.B.Position, Color.red, 100f);
            Debug.DrawLine(tetra.B.Position, tetra.C.Position, Color.red, 100f);
            Debug.DrawLine(tetra.C.Position, tetra.A.Position, Color.red, 100f);

            Debug.DrawLine(tetra.A.Position, tetra.D.Position, Color.red, 100f);
            Debug.DrawLine(tetra.B.Position, tetra.D.Position, Color.red, 100f);
            Debug.DrawLine(tetra.C.Position, tetra.D.Position, Color.red, 100f);
        }
    }



}
