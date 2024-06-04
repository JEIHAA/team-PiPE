using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colidRoomParents : MonoBehaviour 
{

    [SerializeField]
    CollidRoom collidroom;
    // Start is called before the first frame update

    private void Update()
    {
        if(collidroom.returnbool() == true)
        {
            int a = Random.Range(-20, 20);

            int b = Random.Range(-20, 20);

            transform.Translate(new Vector3(a, 0, b));
        }
    }



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
