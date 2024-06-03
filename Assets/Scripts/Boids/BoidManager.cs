using BoidsSimulationOnGPU;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BoidManager : MonoBehaviour
{
  [SerializeField] private int ownerID = -1;
  public int OwnerID { get { return ownerID; } set { ownerID = value; } }
  [SerializeField] private int boidID = 0;
  public int BoidID { get { return boidID; } set { boidID = value; } }

  [SerializeField] private List<BoidsPCPlayerControllerntroller> players = new List<BoidsPCPlayerControllerntroller>();
  [SerializeField] private GameObject[] playerGo;
  [SerializeField] private GameObject owner;
  [SerializeField] private Vector3 tagetPos;
  public Vector3 TagetPos { get { return tagetPos; } }

  private void Awake()
  {
    playerGo = GameObject.FindGameObjectsWithTag("Player");;
    tagetPos = this.transform.position;

    for (int i = 0; i < playerGo.Length; i++) {
        players.Add(playerGo[i].GetComponent<BoidsPCPlayerControllerntroller>());
    }
  }

  private void OnTriggerEnter(Collider _other)
  {
    if (OwnerID != -1)
     return;
    Debug.Log("OnCollisionEnter");
    if (_other.gameObject.layer == LayerMask.NameToLayer("Player"))
    {
      OwnerID = _other.gameObject.GetComponent<BoidsPCPlayerControllerntroller>().PlayerID;
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
    tagetPos = owner.transform.position;
    Debug.Log("owner"+tagetPos);
    tagetPos.y -= 0.5f;
  }

  public int GetOwnerID() { return ownerID; }
  public Vector3 GetTargetPos() { return tagetPos; }
}
