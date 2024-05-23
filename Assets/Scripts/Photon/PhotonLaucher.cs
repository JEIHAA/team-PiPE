using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonLaucher : MonoBehaviourPunCallbacks
{
    [SerializeField] private string gameVersion = "0.0.1";
    [SerializeField] private byte maxPlayerPerRoom = 2;
    [SerializeField] private string nickName = string.Empty;
    [SerializeField] private Button connectButton = null;
    [SerializeField] private TMPro.TMP_InputField inputbox = null;

    private void Awake()
    {
        // 마스터가 PhotonNetwork.LoadLevel()을 호출하면,
        // 모든 플레이어가 동일한 레벨을 자동으로 로드
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        connectButton.interactable = true;
        if(PhotonNetwork.IsConnected)
        {
            inputbox.text = PhotonNetwork.LocalPlayer.NickName;
        }
    }
    // Connect Button이 눌러지면 호출
    public void Connect()
    {
        if(string.IsNullOrEmpty(nickName))
        {
            // 닉네임 창이 비어있을때 뜰 내용
            return;
        }

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogFormat("Connect: {0}", gameVersion);

            PhotonNetwork.GameVersion = gameVersion;
            // 포톤 클라우드에 접속을 시작하는 지점
            // 접속에 성공하면 OnConnectedToMaster 메소드 호출
            PhotonNetwork.ConnectUsingSettings();

            connectButton.interactable = false;
        }
    }

    public void OnValueChagedNickName(string _nickName)
    {
        nickName = _nickName;
        PhotonNetwork.NickName = nickName;
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogFormat("Connected to Master: {0}", nickName);

        if(connectButton.interactable == false)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Disconnted: {0}", cause);

        //connectButton.interactable = true;

        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        SceneManager.LoadScene("Lobby");
        connectButton.interactable = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("JoinRandomFailed({0}): {1}", returnCode, message);

        connectButton.interactable = true;

        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom });
    }

    public void CloseApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}