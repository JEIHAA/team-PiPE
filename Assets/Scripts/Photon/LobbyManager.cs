using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI[] curPlayerList;
    [SerializeField] Button startBtn;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ApplyPlayerListText", RpcTarget.All);
            startBtn.gameObject.SetActive(false);
        }
        startBtn.onClick.AddListener(() => { StartGame(); });
        startBtn.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        SceneManager.LoadScene("Main_Test");
    }


    [PunRPC]
    public void ApplyPlayerListText()
    {
        Debug.Log("Active");
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Debug.Log("Inside");
            if (PhotonNetwork.PlayerList[i].NickName != null)
            {
                Debug.Log("Player: " + PhotonNetwork.PlayerList[i].NickName);
                curPlayerList[i].text = PhotonNetwork.PlayerList[i].NickName;
            }
        }
    }

    [PunRPC]
    public void ClearPlayerList()
    {
        for (int i = 0; i < curPlayerList.Length; i++)
        {
            curPlayerList[i].text = string.Empty;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("ApplyPlayerListText", RpcTarget.All);

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
        {
            startBtn.gameObject.SetActive(true);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        photonView.RPC("ClearPlayerList", RpcTarget.All);
        photonView.RPC("ApplyPlayerListText", RpcTarget.All);

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Title");
    }
}
