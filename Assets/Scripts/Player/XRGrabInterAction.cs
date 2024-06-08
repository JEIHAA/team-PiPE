using BoidsSimulationOnGPU;
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
        grabrb.velocity = Vector3.zero;
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

    public void XRChangeSize(float _charge)
    {
        _charge = _charge * 0.05f;
        _charge = Mathf.Clamp(_charge, 0.05f, 0.3f);
        
        transform.localScale = Vector3.one * _charge;
    }

    public void Throw(Vector3 _dir,float _throwPower)
    {
        if (grabrb == null)
        {
            Debug.LogError("Rigidbody not initialized");
            return;
        }
        grabrb.AddForce(_dir * _throwPower + Vector3.up * 5, ForceMode.Impulse);
    }

}
