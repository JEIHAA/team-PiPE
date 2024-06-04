using BoidsSimulationOnGPU;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoidsPlayerManager : MonoBehaviour
{
  private Queue<GameObject> assignBoidQueue = new Queue<GameObject>();
  public Queue<GameObject> AssignBoidQueue { get { return assignBoidQueue; } set { assignBoidQueue = value; } }



  public int GetHasBoidsNum() {
    Debug.Log($"{this.gameObject.name}'s has boid: {assignBoidQueue.Count}");
    return assignBoidQueue.Count;
  }

  public int[] ShootMissBoid(int _chargeGage, Vector3 _dropPos) 
  {
    GameObject boid = new GameObject();
    BoidManager bm = new BoidManager();
    int[] boidsID = new int[_chargeGage];

    for (int i = 0; i < _chargeGage; ++i) 
    {
      boid = assignBoidQueue.Dequeue();
      bm = boid.GetComponent<BoidManager>();
      bm.OwnerID = -1;
      boidsID[i] = bm.BoidID;
      bm.TargetPos = _dropPos;
      //boid.GetComponentInParent<GPUBoids>().BoidDataArr[bm.BoidID].Position = dropPos;
      Debug.Log($"boidID: {bm.BoidID}, ownerID: {bm.OwnerID}");
    }
    return boidsID;
  }

    public void StealBoids(int _chargeGage, GameObject shooter, GameObject _otherPlayer, int _ownerID)
    {
        GameObject boid = new GameObject();
        BoidManager bm = new BoidManager();
        for (int i = 0; i < _chargeGage; ++i)
        {
            boid = _otherPlayer.GetComponent<BoidsPlayerManager>().assignBoidQueue.Dequeue();
            bm = boid.GetComponent<BoidManager>();
            bm.OwnerID = _ownerID;
            shooter.GetComponent<BoidsPlayerManager>().assignBoidQueue.Enqueue(boid);
            Debug.Log($"boidID: {bm.BoidID}, ownerID: {bm.OwnerID}");
        }
    }
}
