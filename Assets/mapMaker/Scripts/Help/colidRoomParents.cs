using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colidRoomParents : MonoBehaviour 
{

    [SerializeField]
    CollidRoom collidroom;
    // Start is called before the first frame update




    public Transform getTr()
    {
        return transform;
    }
    public Vector3 getBX()
    {
        
        return collidroom.getBoX();
        
        
    }

    public void destroyCollidRoom()
    {
        
        Destroy(collidroom.transform.gameObject);
    }
    public bool returnbool()
    {
        return collidroom.returnbool();
    }
}
