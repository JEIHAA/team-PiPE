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
    [SerializeField] private float throwPower;
    [SerializeField] private Transform grabPos;
    [SerializeField] private float pickRange;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        mainCam = GameObject.FindGameObjectWithTag("PCOrigin").GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                instantiatedBoom = PhotonNetwork.Instantiate("Boom", new Vector3(grabPos.transform.position.x, grabPos.transform.position.y, grabPos.transform.position.z), Quaternion.identity);
                boom = instantiatedBoom.GetComponent<XRGrabInterAction>();
                boom.XRGrab();
            }
            if (Input.GetMouseButton(0))
            {
                chargeGage += Time.deltaTime;
                chargeGage = Mathf.Clamp(chargeGage, 0.1f, 2f);
                boom.XRChangeSize(chargeGage);
                HoldObject(instantiatedBoom);
            }
            if (Input.GetMouseButtonUp(0))
            {
                boom.XRRealease();
                boom.Throw(mainCam.transform, throwPower);
                instantiatedBoom = null;
                chargeGage = 0;
            }
            if (Input.GetMouseButton(1))
            {

            }
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
    private void HoldObject(GameObject _grabObject)
    {
        _grabObject.transform.position = grabPos.transform.position;

    }

    /*private void RealeaseObject()
    {
        if (pickObject != null)
        {
            pickReceiver.PCRealease();
            pickReceiver.Throw(mainCam.transform.forward, throwPower);
        }
    }*/
}
