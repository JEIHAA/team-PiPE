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
        setbool(true);
        int a = Random.Range(-20, 20);

        int b = Random.Range(-20, 20);

        transform.Translate(new Vector3(a, 0, b));
        
    }

    private void OnCollisionExit(Collision collision)
    {
        setbool(false);
        
    }

    public void setbool(bool col)
    {
        colid = col;
    }

    public bool returnbool()
    {
        return colid;
    }

    public Transform getTr()
    {
        return transform;
    }
    public Vector3 getBX()
    {
        BoxCollider bx = transform.GetComponent<BoxCollider>();
        
        return bx.size;
    }

}