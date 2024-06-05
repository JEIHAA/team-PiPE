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
    private GPUBoids boid;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        mainCam = GameObject.FindGameObjectWithTag("PCOrigin").GetComponentInChildren<Camera>();
        boidsPlayerManager = GetComponent<BoidsPlayerManager>();
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                instantiatedBoom = PhotonNetwork.Instantiate("Boom", new Vector3(grabPos.transform.position.x, grabPos.transform.position.y, grabPos.transform.position.z), Quaternion.identity);
                boom = instantiatedBoom.GetComponent<XRGrabInterAction>();
                PV.RPC("GrabBomb", RpcTarget.All);
            }
            if (Input.GetMouseButton(0))
            {
                chargeGage += Time.deltaTime * 5f;
                Debug.Log(boidsPlayerManager.GetHasBoidsNum());
                chargeGage = Mathf.Clamp(chargeGage, 1f, boidsPlayerManager.GetHasBoidsNum());
                boom.XRChangeSize(chargeGage);
                PV.RPC("HoldObject", RpcTarget.All,boom.gameObject);
            }
            if (Input.GetMouseButtonUp(0))
            {
                boom.XRRealease();
                PV.RPC("ThrowBomb", RpcTarget.All);
                instantiatedBoom = null;
                boom = null;
                boid.ChargeGage = (int)chargeGage;
                chargeGage = 0;
            }
            if (Input.GetMouseButton(1))
            {

            }
        }
        
    }

    [PunRPC]
    private void ThrowBomb()
    {
        boom.Throw(mainCam.transform, throwPower);
    }

    [PunRPC]
    private void GrabBomb()
    {
        boom.XRGrab();
    }

    [PunRPC]
    private void HoldObject(GameObject _grabObject)
    {
        _grabObject.transform.position = grabPos.transform.position;
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
