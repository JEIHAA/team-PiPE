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
    public GameObject PCOrigin;
    public MapGenerate2D maps;
    public BoidsGameObjectGenerator boids;
    public Transform LeftHandController;
    public Transform RightHandController;
    private int setPlayerID = 0;

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
            MaxPlayers = 2
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
            PCOrigin.SetActive(false);
            //go.GetComponent<XRPlayerController>().PlayerID = PhotonNetwork.LocalPlayer.ActorNumber;

        }
        else
        {

            go = PhotonNetwork.Instantiate("PC_Player", new Vector3(originPos.x, 0, originPos.y), Quaternion.identity);
            XROrigin.SetActive(false);

           // go.GetComponent<PCPlayerController>().PlayerID = PhotonNetwork.LocalPlayer.ActorNumber;

        }


        //PhotonNetwork.Instantiate("ObjectPool", new Vector3(originPos.x, 0, originPos.y), Quaternion.identity);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient) // 이 내용들은 게임 스타트 버튼을 누르면 해야 함. 플레이어 생성도 고민해야 함
        {
            //photonView.RPC("GenerateMap", RpcTarget.AllBuffered);
            photonView.RPC("StartBoidsGenerate", RpcTarget.AllBuffered);
            photonView.RPC("PlayerSetting", RpcTarget.AllBuffered);
            
        }
    }

    [PunRPC]
    public void SetPlayerID() 
    {
        Debug.Log("SetPlayerID Active");
        setPlayerID++;
    }

    [PunRPC]
    public void GenerateMap()
    {
        Debug.Log("ConnManager Map Generate");
        maps.StartMapGenerator();
    }

    [PunRPC]
    public void StartBoidsGenerate()
    {
        boids.StartSpawnBoids();
    }


}
