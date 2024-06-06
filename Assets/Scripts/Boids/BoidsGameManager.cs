using BoidsSimulationOnGPU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BoidsGameManager : MonoBehaviourPun
{
  [SerializeField] private BoidsPlayerManager[] pm;
    [SerializeField] private BoidsGameObjectGenerator generate;
    [SerializeField] private GPUBoids boid;

    private void Awake()
    {
        //generate.OnFinishedGenerateCallBack = ActiveBoids;
        generate.ActivateBoidsCallback = ActiveBoids;
        boid.gameObject.SetActive(false);
    }

    
    public void ActiveBoids()
    {
        photonView.RPC("ObjectCtrl", RpcTarget.All);
    }
    [PunRPC]
    public void ObjectCtrl()
    {
        boid.gameObject.SetActive(true);
    }
    private int GetPlayerHasTotalBoids() {
    int totalBoids = 0;
    for (int i = 0; i < pm.Length; i++)
    {
      totalBoids += pm[i].GetHasBoidsNum();
    }
    Debug.Log($"totalBoids: {totalBoids}");
    return totalBoids;
  }
}
