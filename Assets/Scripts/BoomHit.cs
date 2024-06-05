using BoidsSimulationOnGPU;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BoidsSimulationOnGPU.GPUBoids;

public class BoomHit : MonoBehaviour
{
    private GPUBoids boid;
    private BoidsPlayerManager boidPM;
    private BoidsPCPlayerController myID;
    private PCPlayerController pcController;
    private XRPlayerController xrController;
    private int chargeGage;

    BoidBomb dataStr = new BoidBomb();


  private void Awake()
  {
      boid = GameObject.FindGameObjectWithTag("Boid").GetComponentInChildren<GPUBoids>();
      boidPM = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<BoidsPlayerManager>();
      myID = GetComponent<BoidsPCPlayerController>();
  }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� �������� ������ ����
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            ContactPoint[] contactPoints = collision.contacts;
            Debug.Log("       " + contactPoints.Length);

            foreach (ContactPoint contact in contactPoints)
            {
                // contact.point�� �浹 ������ ���� ��ǥ�� ��Ÿ���ϴ�.
                Debug.Log("       " + contact);
                dataStr.DropPos = contact.point;
                dataStr.HitPlayer = false;
                Debug.Log("Collision point: ");
                Debug.Log("Collision point: " + dataStr.DropPos);
            }
        }
        

        if (collision.gameObject.CompareTag("Player"))
        {
            pcController = collision.gameObject.GetComponent<PCPlayerController>();
            xrController = collision.gameObject.GetComponent<XRPlayerController>();

            dataStr.HitPlayer = true;
            Debug.Log(boid.ChargeGage);
            Debug.Log(collision.gameObject.name);
            //Debug.Log(myID.PlayerID);
            if(xrController != null )
            {
                dataStr.HitPlayer = true;
                dataStr.HitPlayerID = xrController.SendId();
                 boidPM.StealBoids(boid.ChargeGage, xrController.gameObject, collision.gameObject, myID.PlayerID);
             }
            else if(pcController != null)
            {
                dataStr.HitPlayer = true;
                dataStr.HitPlayerID = pcController.SendId();
                boidPM.StealBoids(boid.ChargeGage, pcController.gameObject, collision.gameObject, myID.PlayerID);
            }
        }
        boid.SetBoidBomb(dataStr);
        if(!dataStr.HitPlayer)
           boidPM.ShootMissBoid(boid.ChargeGage, dataStr.DropPos);


    Destroy(gameObject);
    }
}
