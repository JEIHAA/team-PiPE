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
    private GameObject[] playerGoList;

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
       /* photonView.RPC("GenerateMap", RpcTarget.AllBuffered);
        yield return StartCoroutine(WaitForProcess());
        isFinished = false;*/
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

    [PunRPC]
    public void ApplyPlayerList()
    {

        for (int i = 0; i < playerGoList.Length; i++)
         {
             playerGoList[i].GetComponent<BoidsPlayerManager>().PlayerID = playerGoList[i].GetComponent<PhotonView>().OwnerActorNr - 1;
         }
        EndProcess();
    }


    [PunRPC]
    public void StartPlayerSpawn()
    {
        StartCoroutine(SpawnPlayer());
    }

    [PunRPC]
    private IEnumerator SpawnPlayer()
    {
        Vector2 pos = Random.insideUnitCircle * 2.0f;
        if (client != null && client.isPc == false)
        {
            PhotonNetwork.Instantiate("VR_Player", new Vector3(pos.x, 0, pos.y), Quaternion.identity, 0);
            PCOrigin.SetActive(false);
        }

        if (client != null && client.isPc == true)
        {
            PhotonNetwork.Instantiate("PC_Player", new Vector3(pos.x, 2, pos.y), Quaternion.identity, 0);
            XROrigin.gameObject.SetActive(false);
        }
        
        yield return StartCoroutine(WaitForSpawn());

        

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

    private IEnumerator WaitForSpawn()
    {
        
        while (true)
        {
            GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
            if (go.Length == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                Debug.Log("Wait... " + go.Length);
                playerGoList = go;
                EndProcess();
                yield break;
            }
            yield return null;
        }
        
    }
    
}
