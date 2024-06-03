using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRBoidsTrigger : MonoBehaviour
{
    public float chargeGage;
    private XRGrabInterAction boom;
    private float triggerOn;
    private bool isTriggerOn;
    private GameObject instantiatedBoom;
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private float throwPower = 20f;
    private PhotonView PV;
    private GameObject controller;
    private void Awake()
    {
        PV = GetComponentInParent<PhotonView>();
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            XRTriggerCheck();
        }
        
    }
    private void XRTriggerCheck()
    {
        triggerOn = actionAsset.actionMaps[5].actions[2].ReadValue<float>();
        if (instantiatedBoom != null)
        {
            instantiatedBoom.transform.position = controller.transform.position;
            instantiatedBoom.transform.rotation = controller.transform.rotation;
        }

        if (isTriggerOn && triggerOn == 1)
        {
            chargeGage += Time.deltaTime * 5f;
            Debug.Log("Check count");
            
        }
        else if (isTriggerOn && triggerOn == 0)
        {
            chargeGage = 0;
        }
        else if(!isTriggerOn && triggerOn == 0)
        {
            Debug.Log("Trigger check Check");
            if(instantiatedBoom != null)
            {
                Debug.Log("Throw check");
                Debug.Log("Throw Check" + boom);
                boom.XRRealease();
                boom.Throw(controller.transform, throwPower);
                instantiatedBoom = null;
            }
        }
    }

    private IEnumerator ChargingBoids()
    {
        while (isTriggerOn)
        {
            chargeGage += Time.deltaTime;
            chargeGage = Mathf.Clamp(chargeGage, 0.1f, 2f);
            yield return null;
        }
        yield break;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            Debug.Log("Controller!! Enter!1");
            controller = other.gameObject;
            isTriggerOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            Debug.Log("Controller!! EXit!!");
            isTriggerOn = false;
            if(chargeGage>=1 && instantiatedBoom ==null)
            {
                instantiatedBoom = PhotonNetwork.Instantiate("Boom", new Vector3(controller.transform.position.x, controller.transform.position.y, controller.transform.position.z), Quaternion.identity);
                boom = instantiatedBoom.GetComponent<XRGrabInterAction>();
                boom.XRGrab();
                boom.XRChangeSize(chargeGage);
                // 차지 게이지 보내주면됨
                chargeGage = 0;
            }

        }
    }
}
