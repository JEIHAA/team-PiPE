using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.XR.CoreUtils;
using ExitGames.Client.Photon;

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

    [PunRPC]
    public void ApplyPlayerList()
    {
        // 현재 방에 접속해 있는 플레이어의 수

        // 현재 생성되어 있는 모든 포톤뷰 가져오기
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();

        // 매번 재정렬을 하는게 좋으므로 플레이어 게임오브젝트 리스트를 초기화
        playerGoList.Clear();

        // 현재 생성되어 있는 포톤뷰 전체와
        // 접속중인 플레이어들의 액터넘버를 비교해,
        // 액터넘버를 기준으로 플레이어 게임오브젝트 배열을 채움
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            // 키는 0이 아닌 1부터 시작
            int key = i + 1;
            for (int j = 0; j < photonViews.Length; ++j)
            {
                // 만약 PhotonNetwork.Instantiate를 통해서 생성된 포톤뷰가 아니라면 넘김
                if (photonViews[j].isRuntimeInstantiated == false) continue;
                // 만약 현재 키 값이 딕셔너리 내에 존재하지 않는다면 넘김
                if (PhotonNetwork.CurrentRoom.Players.ContainsKey(key) == false) continue;

                // 포톤뷰의 액터넘버
                int viewNum = photonViews[j].Owner.ActorNumber;
                // 접속중인 플레이어의 액터넘버
                int playerNum = PhotonNetwork.CurrentRoom.Players[key].ActorNumber;  

                // 액터넘버가 같은 오브젝트가 있다면,
                if (viewNum == playerNum && photonViews[j].gameObject.TryGetComponent<CharacterController>(out CharacterController Con))
                {
                    // 실제 게임오브젝트를 배열에 추가
                    playerGoList.Add(photonViews[j].gameObject);
                    // 게임오브젝트 이름도 알아보기 쉽게 변경
                    playerGoList[playerNum - 1].name = "Player_" + photonViews[j].Owner.NickName;
                }
            }
        }

        for (int i = 0; i < playerGoList.Count; i++)
        {
           playerGoList[i].GetComponent<BoidsPlayerManager>().PlayerID = playerGoList[i].GetComponent<PhotonView>().OwnerActorNr;
        }
        EndProcess();
    }
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
