using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    private DontDestoryObject client;
    public XRPlayerSync playersync;
    public Transform XROrigin;
    private GameObject player;

    private void Awake()
    {
        client = FindObjectOfType<DontDestoryObject>();
    }

    private void Start()
    {
        if (client != null && client.isPc == false)
        {
            player = PhotonNetwork.Instantiate("VR_Player", XROrigin.position, XROrigin.rotation, 0);
            //playersync.SetPlayer(player);
        }

        if (client != null && client.isPc == true)
        {
            Vector2 pos = Random.insideUnitCircle * 2.0f;
            player = PhotonNetwork.Instantiate("PC_Player", new Vector3(pos.x, 2, pos.y), Quaternion.identity, 0);
            XROrigin.gameObject.SetActive(false);
            
        }
        

    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Destroy(player);
    }
}
