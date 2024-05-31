using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRPlayerSync : MonoBehaviour
{
    private GameObject player = null;

    private void Update()
    {
        if (player != null)
        {
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
        }
    }

    public void SetPlayer(GameObject go)
    {
        player = go;
    }


}
