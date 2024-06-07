using BoidsSimulationOnGPU;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoidsPlayerManager : MonoBehaviour
{
  [SerializeField] private int playerID;
  public int PlayerID { get { return playerID; } set { playerID = value; } }

  [SerializeField] private Queue<GameObject> assignBoidQueue = new Queue<GameObject>();
  public Queue<GameObject> AssignBoidQueue { get { return assignBoidQueue; } set { assignBoidQueue = value; } }

   public int GetHasBoidsNum() 
   {
       return assignBoidQueue.Count;
   }

    public void DebougLogBoidsNum() {
        Debug.Log($"{this.playerID}'s has boid: {assignBoidQueue.Count}");
    }

  public int[] ShootMissBoid(int _chargeGage, Vector3 _dropPos) 
  {
    GameObject boid;
    BoidManager bm = new BoidManager();
    int[] boidsID = new int[_chargeGage];

    for (int i = 0; i < _chargeGage; ++i) 
    {
      Debug.Log($"i: {i}, dropPos: {_dropPos}");
      boid = this.assignBoidQueue.Dequeue();
      bm = boid.GetComponent<BoidManager>();
      bm.OwnerID = -1;
      boidsID[i] = bm.BoidID;
      bm.TargetPos = _dropPos;
      //boid.GetComponentInParent<GPUBoids>().BoidDataArr[bm.BoidID].Position = dropPos;
      Debug.Log($"boidID: {bm.BoidID}, ownerID: {bm.OwnerID}");
      DebougLogBoidsNum();
    }
    return boidsID;
  }

    public void StealBoids(int _chargeGage, GameObject shooter, int _shooterID, GameObject _hitPlayer)
    {
        GameObject boid = new GameObject();
        BoidManager bm = new BoidManager();
        for (int i = 0; i < _chargeGage; ++i)
        {
            boid = _hitPlayer.GetComponent<BoidsPlayerManager>().assignBoidQueue.Dequeue();
            bm = boid.GetComponent<BoidManager>();
            bm.OwnerID = _shooterID;
            shooter.GetComponent<BoidsPlayerManager>().assignBoidQueue.Enqueue(boid);
            Debug.Log($"boidID: {bm.BoidID}, ownerID: {bm.OwnerID}");
        }
    }
}
