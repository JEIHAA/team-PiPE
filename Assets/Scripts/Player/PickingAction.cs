using BoidsSimulationOnGPU;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PickingAction : MonoBehaviour
{
   public float chargeGage;
   private Camera mainCam;
   private GameObject pickObject;
   private XRGrabInterAction pickReceiver;
   private GameObject instantiatedBoom;
   private PhotonView PV;
   private XRGrabInterAction boom;
   private BoidsPlayerManager boidsPlayerManager;
   [SerializeField] private float throwPower;
   [SerializeField] private Transform grabPos;
   [SerializeField] private float pickRange;
   [SerializeField] private GameObject bombPrefab;
   [SerializeField] private GPUBoids boid;
   private bool isbuttonClicked = false;
   private Transform otherPlayerCamPos;
   private void Awake()
   {
      PV = GetComponent<PhotonView>();
      mainCam = GameObject.FindGameObjectWithTag("PCOrigin").GetComponentInChildren<Camera>();
      boidsPlayerManager = GetComponent<BoidsPlayerManager>();
      Debug.Log("Awake: "+boidsPlayerManager.gameObject.name+", "+ boidsPlayerManager.PlayerID);
      StartCoroutine(GetBoids());
   }
   private void Update()
   {
      if (PV.IsMine)
      {
         if (boidsPlayerManager.GetHasBoidsNum() > 0)
         {
            if (Input.GetMouseButtonDown(0))
            {
               PV.RPC("OnClicked", RpcTarget.All);
            }
            if (Input.GetMouseButtonUp(0))
            {
               PV.RPC("ExitClicked", RpcTarget.All);
            }
         }
         PV.RPC("UpdateOtherPlayerCamPos", RpcTarget.Others, mainCam.transform.position, mainCam.transform.rotation);
      }
      SpawnBomb();
   }
   private void SpawnBomb()
   {
      if (isbuttonClicked && instantiatedBoom == null)
      {
         instantiatedBoom = Instantiate(bombPrefab, new Vector3(grabPos.transform.position.x, grabPos.transform.position.y, grabPos.transform.position.z), Quaternion.identity);
         instantiatedBoom.GetComponent<BoomHit>().ShooterID = boidsPlayerManager.PlayerID;
         Debug.Log("instantiatedBoom.GetComponent<BoomHit>().ShooterID: " + instantiatedBoom.GetComponent<BoomHit>().ShooterID);
         boom = instantiatedBoom.GetComponent<XRGrabInterAction>();
         boom.XRGrab();
      }
      if (isbuttonClicked)
      {
         chargeGage += Time.deltaTime * 5f;
         //Debug.Log(boidsPlayerManager.GetHasBoidsNum());
         chargeGage = Mathf.Clamp(chargeGage, 1f, boidsPlayerManager.GetHasBoidsNum() ==1 ? boidsPlayerManager.GetHasBoidsNum() : boidsPlayerManager.GetHasBoidsNum()-1);
         Debug.Log("chargeGage: "+chargeGage);
         boom.XRChangeSize(chargeGage);
         boom.transform.position = grabPos.transform.position;
      }
      if (!isbuttonClicked && instantiatedBoom != null)
      {
         boom.XRRealease();
         instantiatedBoom.transform.SetParent(null);
         Transform throwDirection = PV.IsMine ? mainCam.transform : otherPlayerCamPos;
         boom.Throw(throwDirection, throwPower);
         instantiatedBoom = null;
         boom = null;
         boid.ChargeGage = (int)chargeGage;
         chargeGage = 0;
      }
   }
   [PunRPC]
   private void OnClicked()
   {
      isbuttonClicked = true;
   }
   [PunRPC]
   private void ExitClicked()
   {
      isbuttonClicked = false;
   }
   [PunRPC]
   private void UpdateOtherPlayerCamPos(Vector3 position, Quaternion rotation)
   {
      if (otherPlayerCamPos == null)
      {
         GameObject cameraObj = new GameObject("OtherPlayerCamera");
         otherPlayerCamPos = cameraObj.transform;
      }
      otherPlayerCamPos.position = position;
      otherPlayerCamPos.rotation = rotation;
   }
   private IEnumerator GetBoids()
   {
      while (true)
      {
         //GPUBoids bo = GameObject.FindGameObjectWithTag("Boid").GetComponent<GPUBoids>();
         GPUBoids bo = FindObjectOfType<GPUBoids>();
         if (bo == null)
         {
            Debug.Log("Bo is NULL!!");
         }
         if (bo != null)
         {
            Debug.Log("Bo Find!");
            boid = bo;
            yield break;
         }
         yield return null;
      }
   }
   /*private void Picking()
   {
       RaycastHit hit;
       Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
       if (Physics.Raycast(ray, out hit, pickRange))
       {
           pickObject = hit.collider.gameObject;
           Debug.Log(pickObject.name);
           pickObject.transform.position = grabPos.transform.position;
           pickReceiver = pickObject.GetComponent<PCGrabInterAction>();
           pickReceiver.PCGrab();
       }
   }*/
   /*private void RealeaseObject()
   {
       if (pickObject != null)
       {
           pickReceiver.PCRealease();
           pickReceiver.Throw(mainCam.transform.forward, throwPower);
       }
   }*/
}