using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using System.IO;
using System.Text;
using System;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.UI;

public class MapTransmission
{
    public float posX { get; set; }
    public float posZ { get; set; }
    public float sizeX { get; set; }
    public float sizeZ { get; set; }
    public int index { get; set; }

    public static byte[] Serialize(object customobject)
    {
        MapTransmission ct = (MapTransmission)customobject;

        MemoryStream ms = new MemoryStream((sizeof(float) * 4) + sizeof(int));
        ms.Write(BitConverter.GetBytes(ct.posX), 0, sizeof(float));
        ms.Write(BitConverter.GetBytes(ct.posZ), 0, sizeof(float));
        ms.Write(BitConverter.GetBytes(ct.sizeX), 0, sizeof(float));
        ms.Write(BitConverter.GetBytes(ct.sizeZ), 0, sizeof(float));
        ms.Write(BitConverter.GetBytes(ct.index), 0, sizeof(int));

        return ms.ToArray();
    }

    public static object Deserialize(byte[] bytes)
    {
        MapTransmission ct = new MapTransmission();
        ct.posX = BitConverter.ToSingle(bytes, 0);
        ct.posZ = BitConverter.ToSingle(bytes, sizeof(float));
        ct.sizeX = BitConverter.ToSingle(bytes, (sizeof(float) * 2));
        ct.sizeZ = BitConverter.ToSingle(bytes, (sizeof(float) * 3));
        ct.index = BitConverter.ToInt16(bytes, (sizeof(float) * 4));
        return ct;
    }
}

public class MapTransfor : MonoBehaviourPun
{
    public List<MapTransmission> RecivedRooms = new List<MapTransmission>();
    private void Awake()
    {
        PhotonPeer.RegisterType(typeof(MapTransmission), 0, MapTransmission.Serialize, MapTransmission.Deserialize);
    }

    public void SendMapToNetwork(MapTransmission _custom)
    {
        
        photonView.RPC("SendCustomRPC", RpcTarget.OthersBuffered, _custom);
    }

    [PunRPC]
    public void SendCustomRPC(MapTransmission _customType)
    {
        RecivedRooms.Add(_customType);
    }
}
