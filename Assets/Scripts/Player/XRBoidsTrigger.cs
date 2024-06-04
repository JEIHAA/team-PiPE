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
        // Bomb��� ������ ��ġ ����ȭ
        if (boom != null)
        {
            boom.transform.position = grabPos.transform.position;
            boom.transform.rotation = grabPos.transform.rotation;
        }

        // Ʈ���Ű� �����ְ� �ݶ��̴� �ȿ� ���� �ִٸ� ��¡����
        if (isTriggerOn && triggerOn == 1)
        {
            chargeGage += Time.deltaTime * 5f;
            chargeGage = Mathf.Clamp(chargeGage, 1f, boidsPlayerManager.GetHasBoidsNum());

        }
        // �ݶ��̴� �ȿ������� Ʈ���� ��ҽ� ������ �ʱ�ȭ
        else if (isTriggerOn && triggerOn == 0)
        {
            chargeGage = 0;
        }
        // ������
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
                Debug.Log(chargeGage + " ���������� ����");
                
                boom.XRChangeSize(chargeGage);
                // ���� ������ �����ָ��
                chargeGage = 0;
            }

        }
    }

    
}
