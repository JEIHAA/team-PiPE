using BoidsSimulationOnGPU;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRBoidsTrigger : MonoBehaviourPun
{
    public float chargeGage;
    private XRGrabInterAction boom;
    private float triggerOn;
    private bool isTriggerOn;
    [SerializeField] private GameObject grabPos;
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private float throwPower = 20f;
    [SerializeField] private GameObject bombPrefab;
    private GameObject instantiatedBoom;
    private BoidsPlayerManager boidsPlayerManager;
    private PhotonView PV;
    private bool isbuttonClicked, isCharged = false;
    private Transform otherPlayerConPos;
    private GPUBoids boid;
    private void Awake()
    {
        PV = GetComponentInParent<PhotonView>();
        boidsPlayerManager = GetComponent<BoidsPlayerManager>();
    }
    private void Update()
    {
        if (PV.IsMine)
        {
            triggerOn = actionAsset.actionMaps[5].actions[2].ReadValue<float>();
            if (triggerOn == 1 && isTriggerOn)
            {
                PV.RPC("OnTriggered", RpcTarget.All);
            }
            if (triggerOn == 0 && isTriggerOn)
            {
                PV.RPC("ExitCharge", RpcTarget.All);
            }
            if (triggerOn == 0 && !isTriggerOn && instantiatedBoom != null)
            {
                PV.RPC("ExitClicked", RpcTarget.All);
            }
        }
        SpawnBomb();
    }
    private void SpawnBomb()
    {
        if (instantiatedBoom != null)
        {
            boom.transform.position = grabPos.transform.position;
        }
        if (isbuttonClicked && instantiatedBoom == null)
        {
            Debug.Log("instatiate");
            instantiatedBoom = Instantiate(bombPrefab, new Vector3(grabPos.transform.position.x, grabPos.transform.position.y, grabPos.transform.position.z), Quaternion.identity);
            boom = instantiatedBoom.GetComponent<XRGrabInterAction>();
            boom.XRGrab();
        }
        if (isbuttonClicked && isCharged)
        {
            Debug.Log("ChargE!!");
            chargeGage += Time.deltaTime * 5f;
            Debug.Log(boidsPlayerManager.GetHasBoidsNum());
            chargeGage = Mathf.Clamp(chargeGage, 1f, boidsPlayerManager.GetHasBoidsNum());
            boom.XRChangeSize(chargeGage);
        }
        if (!isbuttonClicked && instantiatedBoom != null)
        {
            Debug.Log("Shoot!");
            boom.XRRealease();
            //변경
            for (int i = 0; i < (int)chargeGage; ++i)
            {
                instantiatedBoom.GetComponent<BoomHit>().BombhasBoid.Enqueue(boidsPlayerManager.ShootBoid());
            }
            instantiatedBoom.GetComponent<BoomHit>().ShooterID = boidsPlayerManager.PlayerID;
            instantiatedBoom.GetComponent<BoomHit>().BombChcargeGage = (int)chargeGage;
            //변경
            boom.Throw(grabPos.transform.up, throwPower);
            instantiatedBoom = null;
            boom = null;
            chargeGage = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            Debug.Log("Controller!! Enter!!");
            isTriggerOn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            Debug.Log("Controller!! EXit!!");
            isTriggerOn = false;
        }
    }
    [PunRPC]
    private void OnTriggered()
    {
        isbuttonClicked = true;
        isCharged = true;
    }
    [PunRPC]
    private void ExitCharge()
    {
        isCharged = false;
    }
    [PunRPC]
    private void ExitClicked()
    {
        isbuttonClicked = false;
        isCharged = false;
    }
    [PunRPC]
    private void UpdateOtherPlayerConPos(Vector3 position, Quaternion rotation)
    {
        if (otherPlayerConPos == null)
        {
            GameObject controllerObj = new GameObject("OtherPlayerCamera");
            otherPlayerConPos = controllerObj.transform;
        }
        otherPlayerConPos.position = position;
        otherPlayerConPos.rotation = rotation;
    }
}
