using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    private DontDestoryObject client;
    private GameObject player;
    public UIManager ui;
    public Transform XROrigin;
    public bool testing;
    public bool test_isPC;

    private void Awake()
    {
        client = FindObjectOfType<DontDestoryObject>();
        
    }

    private void Start()
    {
        /*#region testing_UI
        bool status;
        if (client == null) 
        {
            status = test_isPC;
        }
        else
        {
            status = client.isPc;
        }

        if (testing)
        {
            ui.SetCurClient(status); // << original is below
            ui.LazyAwake();
        }
        #endregion*/

        /*ui.SetCurClient(client.isPc);
        ui.LazyAwake();*/
        

        if (client != null && client.isPc == false)
        {
            Vector2 pos = Random.insideUnitCircle * 2.0f;
            player = PhotonNetwork.Instantiate("VR_Player", new Vector3(pos.x, 0, pos.y),Quaternion.identity, 0);

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
