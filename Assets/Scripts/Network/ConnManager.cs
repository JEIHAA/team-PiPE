using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR;

public class ConnManager : MonoBehaviourPunCallbacks
{
    public bool isVR;
    public GameObject XROrigin;

    public Transform LeftHandController;
    public Transform RightHandController;

    public static bool isPresent()
    {
        var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances<XRDisplaySubsystem>(xrDisplaySubsystems);
        foreach(var xrDisplay in xrDisplaySubsystems)
        {
            if (xrDisplay.running)
            {
                return true;
            }
        }
        return false;
    }

    private void Awake()
    {
        Debug.Log("VR Device = " + isPresent().ToString());
        isVR = isPresent();
    }

    private void Start()
    {
        PhotonNetwork.GameVersion = "0.1";

        int num = Random.Range(0, 1000);
        PhotonNetwork.NickName = "Player" + num;

        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby Connected");
        RoomOptions ro = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 8
        };
        PhotonNetwork.JoinOrCreateRoom("NetTest", ro, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Room Entered");

        Vector2 originPos = Random.insideUnitCircle * 2.0f;
        GameObject go;

        if (isVR == true)
        {
            go = PhotonNetwork.Instantiate("VR_Player", new Vector3(originPos.x, 0, originPos.y), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("PC_Player", new Vector3(originPos.x, 0, originPos.y), Quaternion.identity);
            XROrigin.SetActive(false);
        }
    }
}