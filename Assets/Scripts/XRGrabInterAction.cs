using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRGrabInterAction : MonoBehaviour
{
    private Rigidbody grabrb;
    private SphereCollider sphereCollider;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        grabrb = GetComponent<Rigidbody>();
        
    }


    public void XRGrab()
    {
        if (sphereCollider == null || grabrb == null)
        {
            Debug.LogError("Components not initialized");
            return;
        }

        sphereCollider.enabled = false;
        grabrb.useGravity = false;
        grabrb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    public void XRRealease()
    {
        if (sphereCollider == null || grabrb == null)
        {
            Debug.LogError("Components not initialized");
            return;
        }
        sphereCollider.enabled = true;
        grabrb.useGravity = true;
        grabrb.freezeRotation = false;
    }

    public void Throw(Transform _controller,float _throwPower)
    {
        if (grabrb == null)
        {
            Debug.LogError("Rigidbody not initialized");
            return;
        }
        grabrb.AddForce(_controller.forward * _throwPower + Vector3.up * 5, ForceMode.Impulse);
    }

    /*public void OnCollisionEnter(Collision collision)
    {
        
    }*/
}
