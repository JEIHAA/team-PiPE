using BoidsSimulationOnGPU;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PhotonView))]
public class BoidManager : MonoBehaviourPun
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


  private void Awake()
  {
    playerGo = GameObject.FindGameObjectsWithTag("Player");;
    targetPos = this.transform.position;

    for (int i = 0; i < playerGo.Length; i++) {
        players.Add(playerGo[i].GetComponent<PCPlayerController>());
    }
    }

  private void OnTriggerEnter(Collider _other) // 플레이어 부딫쳤을때
  {
    if (OwnerID != -1)
     return;
    Debug.Log("OnCollisionEnter");
        Debug.Log("Colliosion Detected: " + _other.gameObject.GetComponent<PCPlayerController>().PlayerID);
        if (_other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OwnerID = _other.gameObject.GetComponent<PCPlayerController>().PlayerID;
            _other.GetComponent<BoidsPlayerManager>().AssignBoidQueue.Enqueue(this.gameObject);
            owner = players[ownerID].gameObject;
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
}
