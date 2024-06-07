using BoidsSimulationOnGPU;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BoidsSimulationOnGPU.GPUBoids;
using Photon.Pun;
using System.Data;

public class BoomHit : MonoBehaviourPun
{
    [SerializeField] private GameObject shooter;
    [SerializeField] private int shooterID;
    public int ShooterID { get { return shooterID; } set { shooterID = value; } }

    private BoidsPlayerManager shooterBoidPM;
    private GPUBoids boid;

    //BoidBomb dataStr = new BoidBomb();
    private int hitPlayerID;
    private Vector3 dropPos = Vector3.zero;

    private void Awake()
    {
        boid = GameObject.FindGameObjectWithTag("Boid").GetComponentInChildren<GPUBoids>();
    }

    private void Start()
    {
        shooter = GameManager.Instance().PlayerList[shooterID];
        Debug.Log($"shooter: {shooter} {shooterID}");
        shooterBoidPM = shooter.GetComponent<BoidsPlayerManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("boom OnCollisionEnter: "+ collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player") && !collision.gameObject.GetPhotonView().IsMine)
        {
            Debug.Log($"{collision.gameObject.name} collision enter");
            //shooterBoidPM = collision.gameObject.GetComponent<BoidsPlayerManager>();
            if (shooterBoidPM != null)
            {
                hitPlayerID = shooterBoidPM.PlayerID;
                shooterBoidPM.StealBoids(boid.ChargeGage, shooterBoidPM.gameObject, shooterBoidPM.PlayerID, collision.gameObject);
            }
            Destroy(gameObject);

        }else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // �浹�� �������� ������ ����
            ContactPoint[] contactPoints = collision.contacts;
            Debug.Log("Ground collision Enter");
            foreach (ContactPoint contact in contactPoints)
            {
                // contact.point�� �浹 ������ ���� ��ǥ�� ��Ÿ���ϴ�.
                dropPos = contact.point;
                Debug.Log("Collision point: " + dropPos);
            }
            Destroy(gameObject);

            Debug.Log($"ChargeGage: {boid.ChargeGage}, DropPos: {dropPos}");
            shooterBoidPM.ShootMissBoid(boid.ChargeGage, dropPos);
        }
    }
}
