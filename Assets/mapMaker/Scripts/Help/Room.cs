using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Room 
{

    public Point point;
    public int RoomPrefabIndex;

    public Vector3 Size;

    public float xMin;
    public float xMax;
    public float zMin;
    public float zMax;
    public Transform tr;
    public GameObject g;
    //크기와 위치 저장용
    public Room(Transform transform, Vector3 size)
    {
        point = new Point(transform.position);
        Size = size - new Vector3(2, 0 ,2);


        xMin = transform.position.x - Size.x/2;
        xMax = transform.position.x + Size.x/2;
        zMin = transform.position.z - Size.z/2;
        zMax = transform.position.z + Size.z/2;
    }
    public static bool Intersect(Room a, Room b)
    {
        return !((a.point.Position.x >= (b.point.Position.x + b.Size.x)) || ((a.point.Position.x + a.Size.x) <= b.point.Position.x)
                || (a.point.Position.z >= (b.point.Position.z + b.Size.z)) || ((a.point.Position.z + a.Size.z) <= b.point.Position.z));
    }

    public void RoomMove(Vector3 distance)
    {
        
        tr.Translate(distance);

        point = new Point(tr.position);
    }

}
