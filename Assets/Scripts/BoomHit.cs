using BoidsSimulationOnGPU;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BoidsSimulationOnGPU.GPUBoids;
using Photon.Pun;

public class BoomHit : MonoBehaviourPun
{
    private GPUBoids boid;
    private BoidsPlayerManager boidPM;
    private XRGrabInterAction bombGrab;
    private PhotonView pv;
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
            Destroy(gameObject);
        }
/*        if (collision.gameObject.CompareTag("Player") && !collision.gameObject.GetPhotonView().IsMine)
        {
            boidPM = collision.gameObject.GetComponent<BoidsPlayerManager>();
            if (boidPM != null)
            {
                dataStr.HitPlayer = true;
                dataStr.HitPlayerID = boidPM.PlayerID;
                boidPM.StealBoids(boid.ChargeGage, boidPM.gameObject, boidPM.PlayerID, collision.gameObject );
            }
            Destroy(gameObject);
        }

        
        boid.SetBoidBomb(dataStr);
        if (!dataStr.HitPlayer)
        {
            boidPM.ShootMissBoid(boid.ChargeGage, dataStr.DropPos);
        }*/
        
    }
}
