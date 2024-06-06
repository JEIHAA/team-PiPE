using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.XR.CoreUtils;
using ExitGames.Client.Photon;
using Photon.Pun.Demo.PunBasics;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject XROrigin;
    [SerializeField] GameObject PCOrigin;
    [SerializeField] MapGenerate2D maps;
    [SerializeField] BoidsGameObjectGenerator boids;
    [SerializeField] UIManager ui;




    private DontDestoryObject client;
    private GameObject player;
    private List<GameObject> playerGoList = new List<GameObject>();

    [SerializeField] private int PlayerID = 0;
    private bool isFinished = false;
    private bool GenerateFinished = false;

    private void Awake()
    {
        client = FindObjectOfType<DontDestoryObject>();
        maps.OnFinishedGenerateCallback = EndProcess;
        boids.OnFinishedGenerateCallBack = EndForMaster;

        
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Generate());
        }
        else
        {
            StartCoroutine(WaitForMaster());
        }
        
        
    }

    private IEnumerator Generate()
    {
        photonView.RPC("GenerateMap", RpcTarget.AllBuffered);
        yield return StartCoroutine(WaitForProcess());
        isFinished = false;
        photonView.RPC("SpawnPlayer", RpcTarget.AllBuffered);
        yield return StartCoroutine(WaitForProcess());
        isFinished = false;
        photonView.RPC("ApplyPlayerList", RpcTarget.AllBuffered);
        yield return StartCoroutine(WaitForProcess());
        isFinished = false;
        photonView.RPC("StartBoidsGenerate", RpcTarget.AllBuffered);
        isFinished = false;
        GenerateFinished = false;
        
    }

    public void EndProcess()
    {
        isFinished = true;
    }
    private void EndForMaster()
    {
        GenerateFinished = true;
    }

    private IEnumerator WaitForProcess()
    {
        while (isFinished == false)
        {
            yield return null;
        }
        yield break;
    }

    private IEnumerator WaitForMaster()
    {
        while (GenerateFinished == false)
        {
            yield return null;
        }
        yield break;
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.Destroy(player);
    }

    /*[PunRPC]
    public void ApplyPlayerList()
    {
        // ���� �濡 ������ �ִ� �÷��̾��� ��

        // ���� �����Ǿ� �ִ� ��� ����� ��������
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();

        // �Ź� �������� �ϴ°� �����Ƿ� �÷��̾� ���ӿ�����Ʈ ����Ʈ�� �ʱ�ȭ
        playerGoList.Clear();

        // ���� �����Ǿ� �ִ� ����� ��ü��
        // �������� �÷��̾���� ���ͳѹ��� ����,
        // ���ͳѹ��� �������� �÷��̾� ���ӿ�����Ʈ �迭�� ä��
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            Debug.Log("PlayerCount: " + PhotonNetwork.CurrentRoom.PlayerCount);
            // Ű�� 0�� �ƴ� 1���� ����
            int key = i + 1;
            for (int j = 0; j < photonViews.Length; ++j)
            {
                // ���� PhotonNetwork.Instantiate�� ���ؼ� ������ ����䰡 �ƴ϶�� �ѱ�
                if (photonViews[j].isRuntimeInstantiated == false) continue;
                // ���� ���� Ű ���� ��ųʸ� ���� �������� �ʴ´ٸ� �ѱ�
                if (PhotonNetwork.CurrentRoom.Players.ContainsKey(key) == false) continue;

                Debug.Log("photonView Name: " + photonViews[j].name);
                // ������� ���ͳѹ�
                int viewNum = photonViews[j].CreatorActorNr;
                // �������� �÷��̾��� ���ͳѹ�
                int playerNum = PhotonNetwork.CurrentRoom.Players[key].ActorNumber;
                //Debug.Log("J: " + j + " key: " + key);
                //Debug.Log("photonView Owner ActorNumber: " + viewNum + " currentRoom player actornumber: " + playerNum);
                // ���ͳѹ��� ���� ������Ʈ�� �ִٸ�,
                if (viewNum == playerNum && photonViews[j].gameObject.TryGetComponent<CharacterController>(out CharacterController Con))
                {
                    Debug.Log(photonViews[j].gameObject);
                    // ���� ���ӿ�����Ʈ�� �迭�� �߰�
                    playerGoList.Add(photonViews[j].gameObject);
                    // ���ӿ�����Ʈ �̸��� �˾ƺ��� ���� ����
                    playerGoList[playerNum - 1].name = "Player_" + photonViews[j].Owner.NickName;
                }
            }
        }

        for (int i = 0; i < playerGoList.Count; i++)
        {
           playerGoList[i].GetComponent<BoidsPlayerManager>().PlayerID = playerGoList[i].GetComponent<PhotonView>().OwnerActorNr - 1;
        }
        EndProcess();
    }
*/
    /*[PunRPC]
    public void ApplyPlayerList()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < sortedPlayers.Length; i += 1)
        {
            if (sortedPlayers[i].ActorNumber == actorNumber)
            {
                PlayerID = i; // �ڱ� �ڽ��� ��ȣ�� �� ã�´�.
                break;
            }
        }

        photonView.RPC("ApplyPlayerID", RpcTarget.All, PlayerID);

        
       *//* for (int i = 0; i < playerGoList.Count; i++)
        {
             = playerGoList[i].GetComponent<PhotonView>().OwnerActorNr - 1;
        }*//*
        EndProcess();
    }*/

    /*[PunRPC]
    public void ApplyPlayerID(int _id)
    {
        if (photonView.IsMine)
        {
            foreach (GameObject go in playerGoList)
            {
                if (go.GetComponent<PhotonView>().CreatorActorNr - 1 == PlayerID)
                {
                    go.GetComponent<BoidsPlayerManager>().PlayerID = PlayerID;
                }
            }
        }
        else
        {
            foreach (GameObject go in playerGoList)
            {
                if (go.GetComponent<PhotonView>().CreatorActorNr - 1 == _id)
                {
                    go.GetComponent<BoidsPlayerManager>().PlayerID = _id;
                }
            }
        }

    }*/


    [PunRPC]
    public void SpawnPlayer()
    {
        Vector2 pos = Random.insideUnitCircle * 2.0f;
        if (client != null && client.isPc == false)
        {
            playerGoList.Add(PhotonNetwork.Instantiate("VR_Player", new Vector3(pos.x, 0, pos.y), Quaternion.identity, 0));
            PCOrigin.SetActive(false);
        }

        if (client != null && client.isPc == true)
        {
            playerGoList.Add(PhotonNetwork.Instantiate("PC_Player", new Vector3(pos.x, 2, pos.y), Quaternion.identity, 0));
            XROrigin.gameObject.SetActive(false);
        }
        EndProcess();
    }

    [PunRPC]
    public void GenerateMap()
    {
        maps.StartMapGenerator();
    }

    [PunRPC]
    public void StartBoidsGenerate()
    {
        boids.StartSpawnBoids();
    }

    [PunRPC]
    public void SetPlayerID(GameObject[] players)
    {

    }
}
