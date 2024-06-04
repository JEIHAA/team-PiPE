using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class CollidRoom : MonoBehaviour
{
    public bool colid = false;





    private void OnCollisionStay(Collision collision)
    {
        int a = Random.Range(-20, 20);

        int b = Random.Range(-20, 20);

        transform.Translate(new Vector3(a, 0, b));
        setbool(true);


    }



    private void OnCollisionExit(Collision collision)
    {
        setbool(false);

    }

    private void setbool(bool col)
    {
        colid = col;
    }

    public bool returnbool()
    {
        return colid;
    }
    public Vector3 getBoX()
    {
        BoxCollider bx = transform.GetComponent<BoxCollider>();

        return bx.size;
    }

}