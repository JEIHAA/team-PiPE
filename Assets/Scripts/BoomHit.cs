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
    [SerializeField] private int shooterID;
    public int ShooterID { get { return shooterID; } set { shooterID = value; } }
    [SerializeField] private GameObject shooter;
    private BoidsPlayerManager boidPM;
    BoidBomb dataStr = new BoidBomb();
    private void Awake()
    {
        boid = GameObject.FindGameObjectWithTag("Boid").GetComponentInChildren<GPUBoids>();
    }

    private void Start()
    {
        Debug.Log("shooterID: " + shooterID);
        shooter = GameManager.Instance().PlayerList[shooterID];
        Debug.Log(shooter.GetComponent<BoidsPlayerManager>().PlayerID);
        boidPM = shooter.GetComponent<BoidsPlayerManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("boom OnCollisionEnter: "+ collision.gameObject.name);
        if ((collision.gameObject.CompareTag("Player") && !collision.gameObject.GetPhotonView().IsMine) || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // 충돌한 지점들의 정보를 저장
            ContactPoint[] contactPoints = collision.contacts;
            Debug.Log("       " + contactPoints.Length);
            foreach (ContactPoint contact in contactPoints)
            {
                // contact.point는 충돌 지점의 월드 좌표를 나타냅니다.
                Debug.Log("       " + contact);
                dataStr.DropPos = contact.point;
                dataStr.HitPlayer = false;
                Debug.Log("Collision point: " + dataStr.DropPos);
            }
            Destroy(gameObject);

            boid.SetBoidBomb(dataStr);
            if (!dataStr.HitPlayer)
            {
                Debug.Log($"miss!");
                Debug.Log($"boid.ChargeGage: {boid.ChargeGage}");
                Debug.Log($"dataStr.DropPos: {dataStr.DropPos}");
                boidPM.ShootMissBoid(boid.ChargeGage, dataStr.DropPos);
            }
        }
        /*            if (collision.gameObject.CompareTag("Player"))
                    {
                        boidPM = collision.gameObject.GetComponent<BoidsPlayerManager>();
                        if (boidPM != null)
                        {
                            dataStr.HitPlayer = true;
                            dataStr.HitPlayerID = boidPM.PlayerID;
                            boidPM.StealBoids(boid.ChargeGage, boidPM.gameObject, boidPM.PlayerID, collision.gameObject);
                        }
                        Destroy(gameObject);
                    }*/
    }
}
