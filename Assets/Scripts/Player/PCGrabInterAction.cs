using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCGrabInterAction : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private Rigidbody grabrb;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        grabrb = GetComponent<Rigidbody>();
    }


    public void PCGrab()
    {
        this.sphereCollider.enabled = false;
        this.grabrb.useGravity = false;
        grabrb.freezeRotation = true;
    }

    public void PCRealease()
    {
        this.sphereCollider.enabled = true;
        this.grabrb.useGravity = true;
        grabrb.freezeRotation = false;
    }

    public void Throw(Vector3 _throwDir, float _throwPower)
    {
        grabrb.AddForce(_throwDir * _throwPower + Vector3.up *5, ForceMode.Impulse);
    }
}
