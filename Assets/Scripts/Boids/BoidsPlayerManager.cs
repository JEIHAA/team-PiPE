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

    public void DebugLogBoidsNum() 
    {
        Debug.Log($"{this.playerID}'s has boid: {assignBoidQueue.Count}");
    }

    public void ShootMissBoid(int _chargeGage, Vector3 _dropPos) 
    {
        Debug.Log($"miss!");
        GameObject boid;
        BoidManager bm = new BoidManager();
        int[] boidsID = new int[_chargeGage];

        for (int i = 0; i < _chargeGage; ++i) 
        {
            if (this.assignBoidQueue.Count > 0)
            {
                Debug.Log($"i: {i}, dropPos: {_dropPos}");
                boid = this.assignBoidQueue.Dequeue();
                bm = boid.GetComponent<BoidManager>();
                bm.OwnerID = -1;
                boidsID[i] = bm.BoidID;
                bm.TargetPos = _dropPos;
                //boid.GetComponentInParent<GPUBoids>().BoidDataArr[bm.BoidID].Position = dropPos;
                Debug.Log($"Lose boid !! boidID: {bm.BoidID}, ownerID: {bm.OwnerID}, {PlayerID} hasBoid: {assignBoidQueue.Count}");
            }
            else {
                Debug.Log($"Can't Shoot. this.Player{PlayerID} does not have a void.");
                return;
            }
            
        }
    }

    public void StealBoids(int _chargeGage, GameObject shooter, int _shooterID, GameObject _hitPlayer)
    {
        Debug.Log($"_chargeGage: {_chargeGage}, shooter: {shooter.name}, _shooterID: {_shooterID}, _hitPlayer: {_hitPlayer}");
        Debug.Log($"Hit!");
        GameObject boid;
        BoidsPlayerManager hitPlayer;
        for (int i = 0; i < _chargeGage; ++i)
        {
            hitPlayer = _hitPlayer.GetComponent<BoidsPlayerManager>();
            if (hitPlayer.assignBoidQueue.Count > 0)
            {
                boid = _hitPlayer.GetComponent<BoidsPlayerManager>().assignBoidQueue.Dequeue();
                shooter.GetComponent<BoidsPlayerManager>().assignBoidQueue.Enqueue(boid);
                boid.GetComponent<BoidManager>().OwnerID = _shooterID;
                Debug.Log($"Get Boid !! boidID: {boid.GetComponent<BoidManager>().BoidID}, ownerID: {boid.GetComponent<BoidManager>().OwnerID}");
                Debug.Log($"shooter.{PlayerID} hasBoid: {assignBoidQueue.Count}, get hit player.{hitPlayer.playerID} hasBoid: {hitPlayer.assignBoidQueue.Count}");

                this.DebugLogBoidsNum();
            }
            else {
                Debug.Log($"Can't Steal. Player{hitPlayer.PlayerID} does not have a void");
                return;
            }
            
        }
    }
}
