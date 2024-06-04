using BoidsSimulationOnGPU;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PhotonView))]
public class BoidManager : MonoBehaviourPun, IPunObservable
{
  [SerializeField] private int ownerID = -1;
  public int OwnerID { get { return ownerID; } set { ownerID = value; } } // <
  [SerializeField] private int boidID = 0;
  public int BoidID { get { return boidID; } set { boidID = value; } } // <

  [SerializeField] private GameObject[] playerGo;
  [SerializeField] private List<PCPlayerController> players = new List<PCPlayerController>();
  [SerializeField] private GameObject owner;
  [SerializeField] private Vector3 targetPos;
  public Vector3 TargetPos { get { return targetPos; } set { targetPos = value; } }

    private int ReceiveOwnerID = 0;
    private int ReceiveBoidID = 0;

  private void Awake()
  {
    playerGo = GameObject.FindGameObjectsWithTag("Player");;
    targetPos = this.transform.position;

    for (int i = 0; i < playerGo.Length; i++) {
        players.Add(playerGo[i].GetComponent<PCPlayerController>());
    }

        photonView.Synchronization = ViewSynchronization.UnreliableOnChange;
        photonView.ObservedComponents[0] = this;

        ReceiveOwnerID = OwnerID;
        ReceiveBoidID = BoidID;
    }

  private void OnTriggerEnter(Collider _other) // 플레이어 부딫쳤을때
  {
    if (OwnerID != -1)
     return;
    Debug.Log("OnCollisionEnter");
    /*if (_other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OwnerID = _other.gameObject.GetComponent<BoidsPCPlayerController>().PlayerID;
            _other.GetComponent<BoidsPlayerManager>().AssignBoidQueue.Enqueue(this.gameObject);
            owner = players[ownerID].gameObject;
        }*/

        if (photonView.IsMine && _other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OwnerID = _other.gameObject.GetComponent<PCPlayerController>().PlayerID;
            _other.GetComponent<BoidsPlayerManager>().AssignBoidQueue.Enqueue(this.gameObject);
            owner = players[ownerID].gameObject;
        }
        else if (!photonView.IsMine && _other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("1");
            Debug.Log(ReceiveOwnerID);
            OwnerID = ReceiveOwnerID;
            _other.GetComponent<BoidsPlayerManager>().AssignBoidQueue.Enqueue(this.gameObject);
            owner = players[ReceiveOwnerID].gameObject;
        }

        if (_other.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
    }
  }

    private void FixedUpdate()
  {
    if (OwnerID != -1) {
      SetOwnerPos();
    }
  }

  public Vector3 GetBoidPos() { return this.transform.position; }

  private void SetOwnerPos() {
    targetPos = owner.transform.position;
    targetPos.y -= 0.5f;
  }

    public void OnPhotonSerializeView(PhotonStream _stream, PhotonMessageInfo _info)
    {
        if (_stream.IsWriting)
        {
            _stream.SendNext(OwnerID);
            _stream.SendNext(BoidID);
        }
        else if (_stream.IsReading)
        {
            ReceiveOwnerID = (int)_stream.ReceiveNext();
            ReceiveBoidID = (int)_stream.ReceiveNext();
        }
    }
}
