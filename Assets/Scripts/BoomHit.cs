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
    //����
    private Queue<GameObject> bombhasBoid = new Queue<GameObject>();
    public Queue<GameObject> BombhasBoid { get  { return bombhasBoid; } set { bombhasBoid = value; } }
    private int bombChargeGage;
    public int BombChcargeGage { get { return bombChargeGage; } set { bombChargeGage = value; } }
    //����
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
                StealBoids(bombChargeGage, shooterBoidPM.gameObject, shooterBoidPM.PlayerID, collision.gameObject);
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
            MissBoid(bombChargeGage, dropPos);
        }
    }

    //����
    private void MissBoid(int _chargeGage, Vector3 _dropPos)
    {
        Debug.Log($"miss!");
        GameObject boid;
        BoidManager bm = new BoidManager();
        int[] boidsID = new int[_chargeGage];

        for (int i = 0; i < _chargeGage; ++i)
        {
            
                Debug.Log($"i: {i}, dropPos: {_dropPos}");
                boid = bombhasBoid.Dequeue();
                bm = boid.GetComponent<BoidManager>();
                bm.OwnerID = -1;
                boidsID[i] = bm.BoidID;
                bm.TargetPos = _dropPos;
        }
    }

    private void StealBoids(int _chargeGage, GameObject shooter, int _shooterID, GameObject _hitPlayer)
    {
        Debug.Log($"_chargeGage: {_chargeGage}, shooter: {shooter.name}, _shooterID: {_shooterID}, _hitPlayer: {_hitPlayer}");
        Debug.Log($"Hit!");
        GameObject boid;
        BoidsPlayerManager hitPlayer = _hitPlayer.GetComponent<BoidsPlayerManager>();
        for (int i = 0; i < _chargeGage; ++i)
        {
            if (hitPlayer.AssignBoidQueue.Count > 0)
            {
                boid = _hitPlayer.GetComponent<BoidsPlayerManager>().AssignBoidQueue.Dequeue();
                shooter.GetComponent<BoidsPlayerManager>().AssignBoidQueue.Enqueue(boid);
                boid.GetComponent<BoidManager>().OwnerID = _shooterID;
            }
            else
            {
                Debug.Log($"Can't Steal. Player{hitPlayer.PlayerID} does not have a void");
                return;
            }
        }
        for(int i =0; i < bombhasBoid.Count; ++i)
        {
            shooter.GetComponent<BoidsPlayerManager>().AssignBoidQueue.Enqueue(bombhasBoid.Dequeue());
        }
    }
    //����
}
