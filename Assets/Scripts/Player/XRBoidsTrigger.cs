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
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private float throwPower = 20f;
    [SerializeField] private GameObject grabPos;
    private BoidsPlayerManager boidsPlayerManager;
    private GameObject instantiatedBoom;
    private PhotonView PV;
    private GameObject controller;
    private void Awake()
    {
        PV = GetComponentInParent<PhotonView>();
        boidsPlayerManager = GetComponent<BoidsPlayerManager>();
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
        // Bomb쏘기 전까지 위치 동기화
        if (boom != null)
        {
            boom.transform.position = grabPos.transform.position;
            boom.transform.rotation = grabPos.transform.rotation;
        }

        // 트리거가 눌려있고 콜라이더 안에 들어와 있다면 차징시작
        if (isTriggerOn && triggerOn == 1)
        {
            chargeGage += Time.deltaTime * 5f;
            chargeGage = Mathf.Clamp(chargeGage, 1f, boidsPlayerManager.GetHasBoidsNum());

        }
        // 콜라이더 안에있지만 트리거 취소시 게이지 초기화
        else if (isTriggerOn && triggerOn == 0)
        {
            chargeGage = 0;
        }
        // 던지기
        else if(!isTriggerOn && triggerOn == 0)
        {
            if(boom != null)
            {
                boom.XRRealease();
                boom.Throw(controller.transform, throwPower);
                boom = null;
            }
        }
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
            if(chargeGage>0 && boom ==null)
            {
                instantiatedBoom = PhotonNetwork.Instantiate("Boom", new Vector3(grabPos.transform.position.x, grabPos.transform.position.y, grabPos.transform.position.z), Quaternion.identity);
                boom.XRGrab();
                Debug.Log(chargeGage + " 차지게이지 최종");
                
                boom.XRChangeSize(chargeGage);
                // 차지 게이지 보내주면됨
                chargeGage = 0;
            }

        }
    }

    
}
