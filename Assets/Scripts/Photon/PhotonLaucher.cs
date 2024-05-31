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
        // �����Ͱ� PhotonNetwork.LoadLevel()�� ȣ���ϸ�,
        // ��� �÷��̾ ������ ������ �ڵ����� �ε�
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
    // Connect Button�� �������� ȣ��
    public void Connect()
    {
        if(string.IsNullOrEmpty(nickName))
        {
            // �г��� â�� ��������� �� ����
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
            // ���� Ŭ���忡 ������ �����ϴ� ����
            // ���ӿ� �����ϸ� OnConnectedToMaster �޼ҵ� ȣ��
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
        Application.Quit(); // ���ø����̼� ����
#endif
    }
}