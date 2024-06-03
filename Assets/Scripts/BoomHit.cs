using BoidsSimulationOnGPU;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BoidsSimulationOnGPU.GPUBoids;

public class BoomHit : MonoBehaviour
{
    private GPUBoids boid;
    private PCPlayerController pcController;
    private XRPlayerController xrController;

    BoidBomb dataStr = new BoidBomb();


  private void Awake()
    {
        boid = GameObject.FindGameObjectWithTag("Boid").GetComponentInChildren<GPUBoids>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 지점들의 정보를 저장
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            ContactPoint[] contactPoints = collision.contacts;
            Debug.Log("       " + contactPoints.Length);

            foreach (ContactPoint contact in contactPoints)
            {
                // contact.point는 충돌 지점의 월드 좌표를 나타냅니다.
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

            if(xrController != null )
            {
                dataStr.HitPlayer = true;
                dataStr.HitPlayerID = xrController.SendId();
            }
            else if(pcController != null)
            {
                dataStr.HitPlayer = true;
                dataStr.HitPlayerID = pcController.SendId();

            }
        }
        boid.SetBoidBomb(dataStr);
        
    Destroy(gameObject);
    }

    
}
