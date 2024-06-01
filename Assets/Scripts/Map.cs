using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    
    private CollidRoom[] room;
    private Transform[] roomPos;
    private List<Transform> roomList = new List<Transform>();
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

        for(int i =0; i<room.Length; i++)
        {
            Room newRoom =  new Room(room[i].transform, room[i].getBX());

            newRoom.tr = room[i].getTr();

            ListRoom.Add(newRoom);
        }
        
        return ListRoom;
    }
    

}
