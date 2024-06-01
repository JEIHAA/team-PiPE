using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MapManager : MonoBehaviourPunCallbacks
{
    private MapGenerate2D rooms = null;
    private MapTransfor transfer = null;

    public Button SendTest = null;

    private void Awake()
    {
        rooms = GetComponentInChildren<MapGenerate2D>();
        transfer = GetComponent<MapTransfor>();
        rooms.OnFinishedMapGenerateCallback = SendMap;
        
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            rooms.StartMapGenerator();
        }
    }

    private void SendMap()
    {
        List<Room> roomlist = new List<Room>();
        List<int> roomidx = new List<int>();


        roomlist = rooms.getRooms();
        roomidx = rooms.getRoomidx();
        for (int i = 0; i < roomlist.Count; ++i)
        {
            MapTransmission SendList = new MapTransmission();
            SendList.posX = roomlist[i].tr.position.x;
            SendList.posZ = roomlist[i].tr.position.z;
            SendList.sizeX = roomlist[i].Size.x;
            SendList.sizeZ = roomlist[i].Size.z;
            SendList.index = roomidx[i];
            transfer.SendMapToNetwork(SendList);
        }

        if (transfer.RecivedRooms.Count == roomlist.Count)
        {
            rooms.SetRoomData(transfer.RecivedRooms);
        }

    }



}
