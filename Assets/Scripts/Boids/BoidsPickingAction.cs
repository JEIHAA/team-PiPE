using BoidsSimulationOnGPU;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BoidsPickingAction : MonoBehaviour
{
  public float chargeGage;
  private Camera mainCam;
  private GameObject pickObject;
  private XRGrabInterAction pickReceiver;
  private GameObject instantiatedBoom;
  private PhotonView PV;
  private XRGrabInterAction boom;
  private BoomHit bomb;
  [SerializeField] private float throwPower;
  [SerializeField] private Transform grabPos;
  [SerializeField] private float pickRange;
  [SerializeField] private GameObject prefab;
  private GPUBoids boid;

  private void Awake()
  {
    PV = GetComponent<PhotonView>();
    mainCam = GameObject.FindGameObjectWithTag("PCOrigin").GetComponentInChildren<Camera>();
    boid = GameObject.FindGameObjectWithTag("Boid").GetComponent<GPUBoids>();
    bomb = prefab.GetComponent<BoomHit>();
  }

  private void Update()
  {
    /*if (PV.IsMine)
    {

    }*/
    if (Input.GetMouseButtonDown(0))
    {
      //instantiatedBoom = Instantiate(prefab, new Vector3(grabPos.transform.position.x, grabPos.transform.position.y, grabPos.transform.position.z), Quaternion.identity);
      instantiatedBoom = PhotonNetwork.Instantiate("Boom", new Vector3(grabPos.transform.position.x, grabPos.transform.position.y, grabPos.transform.position.z), Quaternion.identity);
      boom = instantiatedBoom.GetComponent<XRGrabInterAction>();
      boom.XRGrab();
    }
    if (Input.GetMouseButton(0))
    {
      chargeGage += Time.deltaTime;
      chargeGage = Mathf.Clamp(chargeGage, 1f, 10);
      
      boom.XRChangeSize(chargeGage);
      HoldObject(instantiatedBoom);
    }
    if (Input.GetMouseButtonUp(0))
    {
      boom.XRRealease();
      // boom.Throw(mainCam.transform, throwPower);
      instantiatedBoom = null;
      boom = null;
      boid.ChargeGage = (int)chargeGage;
      chargeGage = 0;
    }
    if (Input.GetMouseButton(1))
    {

    }
  }

  private void HoldObject(GameObject _grabObject)
  {
        _grabObject.transform.position = grabPos.transform.position;
  }

  

}
