using BoidsSimulationOnGPU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class BoidsGameObjectGenerator : MonoBehaviourPun
{
    public delegate void OnFinishedGenerate();
    private OnFinishedGenerate onFinishedGenerateCallBack = null;
    public OnFinishedGenerate OnFinishedGenerateCallBack
    {
        set { onFinishedGenerateCallBack = value; }
    }

    [SerializeField] private GPUBoids maxObjectNum;
    [SerializeField] private Transform boidSpawnerParent;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform boidsParent;
    private List<GameObject> boids = new List<GameObject>();
    private bool isFinished = false;

    public Transform[] boidSpawners;

    private void Awake()
    {
        boidSpawners = boidSpawnerParent.GetComponentsInChildren<Transform>();
        
    }
    private IEnumerator WaitForMaster()
    {
        Debug.Log("Waiting Acitive");
        while (isFinished == false)
        {
            Debug.Log("Waiting...");
            yield return null;
        }
        yield break;
    }
    [PunRPC]
    public void SetStatus(bool _input)
    {
        Debug.Log("SetStatus Active");
        isFinished = _input;
    }

    public void StartSpawnBoids()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartBoids();
        }
        else
        {
            StartCoroutine(CheckBoids());
        }
    }

    private void StartBoids()
    {
        Vector3 boidPosition;
        int idx;
        for (int i = 0; i < maxObjectNum.MaxObjectNum; ++i)
        {
            idx = SetRandomPositionTest();
            boidPosition = boidSpawners[idx].position;
            boidPosition.y = 0.5f;
            boids.Add(PhotonNetwork.Instantiate(Path.Combine("BoidsPrefabs", prefab.name), boidPosition, Quaternion.identity));
            boids[i].transform.SetParent(boidsParent);
            boids[i].name = "boid" + i;
            boids[i].GetComponent<BoidManager>().BoidID = i;
        }

        photonView.RPC("SetStatus", RpcTarget.AllBuffered, true);
    }

    private IEnumerator CheckBoids()
    {
        yield return StartCoroutine(WaitForMaster());

        GameObject[] boid = GameObject.FindGameObjectsWithTag("Boids");

        for (int i = 0; i < boid.Length; ++i)
        {
            boids.Add(boid[i]);
            boids[i].transform.SetParent(boidsParent);
            boids[i].name = "boid" + i;
            boids[i].GetComponent<BoidManager>().BoidID = i;
        }
        onFinishedGenerateCallBack?.Invoke();
        yield break;
    }
    

    private int SetRandomPositionTest() // ±Õµî ºÐ¹è
    {
        int len = boidSpawners.Length;
        int idx = Random.Range(1, len);
        return idx;
    }

    private int SetRandomPosition() // ¿ÏÀü ·£´ý ºÐ¹è
    {
        System.Random random = new System.Random((int)System.DateTime.Now.Ticks);
        int len = boidSpawners.Length;
        int randomIdx = random.Next(1, len);
        return randomIdx;
    }

    public List<GameObject> GetBoidsList()
    {
        return boids;
    }
}
