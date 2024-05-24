using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingAction : MonoBehaviour
{
    private Camera mainCam;
    private GameObject pickObject;
    private PCGrabInterAction pickReceiver;
    [SerializeField] private float throwPower;
    [SerializeField] private Transform grabPos;
    [SerializeField] private float pickRange;
    [SerializeField] private LayerMask layerMask;
    private void Awake()
    {
        mainCam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Picking();
        }
        if (Input.GetMouseButton(0))
        {
            HoldObject();
        }
        if (Input.GetMouseButtonUp(0))
        {
            RealeaseObject();
            pickObject = null;
        }
        if (Input.GetMouseButton(1))
        {

        }
    }

    private void Picking()
    {
        RaycastHit hit;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, pickRange, layerMask))
        {
            pickObject = hit.collider.gameObject;
            Debug.Log(pickObject.name);
            pickObject.transform.position = grabPos.transform.position;
            pickReceiver = pickObject.GetComponent<PCGrabInterAction>();
            pickReceiver.PCGrab();
        }
    }
    private void HoldObject()
    {
        if(pickObject!= null)
        {
            pickObject.transform.position = grabPos.transform.position;
        }
    }

    private void RealeaseObject()
    {
        if (pickObject != null)
        {
            pickReceiver.PCRealease();
            pickReceiver.Throw(mainCam.transform.forward, throwPower);
        }
    }
}
