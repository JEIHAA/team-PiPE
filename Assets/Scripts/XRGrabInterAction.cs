using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRGrabInterAction : MonoBehaviour
{
    private Rigidbody grabrb;
    private SphereCollider sphereCollider;
    [SerializeField] private Transform player;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        grabrb = GetComponent<Rigidbody>();
    }


    public void XRGrab()
    {
        sphereCollider.enabled = false;
        this.grabrb.useGravity = false;
        grabrb.freezeRotation = true;
    }
    public void XRRealease()
    {
        this.sphereCollider.enabled = true;
        this.grabrb.useGravity = true;
        grabrb.freezeRotation = false;
    }

    public void Throw(float _throwPower)
    {
        grabrb.AddForce(player.transform.forward * _throwPower + Vector3.up * 5, ForceMode.Impulse);
    }

    /*public void OnCollisionEnter(Collision collision)
    {
        
    }*/
}
