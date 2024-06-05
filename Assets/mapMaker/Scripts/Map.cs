using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    private CollidRoom[] room;
    private colidRoomParents[] roomParents;
    private Transform[] roomPos;
    private List<Transform> roomList = new List<Transform>();
    System.Random random = new System.Random();
    public List<Transform> GetCollidRoom()
    {
        roomPos = GetComponentsInChildren<Transform>();
        
        for(int i =0; i<roomPos.Length; i++)
        {
            if (roomPos[i].gameObject.layer == 7 && roomPos[i] != transform)
            {
                roomList.Add(roomPos[i]);
            }


        }
        return roomList;
    }

    public bool returnBool()
    {
        room = transform.GetComponentsInChildren<CollidRoom>();

        

        foreach(CollidRoom r in room)
        {
            if(r.returnbool()) return true;    
        }
        return false;
    }


    
    public List<Room> returnList()
    {
        room = GetComponentsInChildren<CollidRoom>();

        List<Room> ListRoom = new List<Room>();

        for (int i = 0; i < room.Length; i++)
        {
            Room newRoom = new Room(room[i].transform.position, room[i].getBoX());

            newRoom.tr = room[i].transform;




            ListRoom.Add(newRoom);

            Destroy(room[i].GetComponent<CollidRoom>());
        }

        return ListRoom;
    }
    public List<Vector3> returnFlag()
    {
        List<Vector3> result = new List<Vector3>();
        Transform[] flagarr = transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in flagarr)
        {
            if (t.gameObject.layer == 12)
            {
                double a = random.NextDouble();
                if (a < 0.3)
                {
                    Destroy(t.gameObject);
                }
                else
                {
                    result.Add(t.position);
                }
            }
        }





        return result;
    }

}
