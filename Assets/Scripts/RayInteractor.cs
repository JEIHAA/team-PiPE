using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RayInteractor : MonoBehaviour
{
    public XRRayInteractor xri;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private float throwPower = 20f;
    [SerializeField] private XRGrabInterAction boom;
    
    private void Start()
    {
        xri.selectEntered.AddListener(SelectEvent);
        xri.selectExited.AddListener(SeletExit);
        
    }

    private void Update()
    {
        RayCheck();
    }

    private void SelectEvent(SelectEnterEventArgs args)
    {
        boom.XRGrab();
    }

    private void SeletExit(SelectExitEventArgs args)
    {
        boom.XRRealease();
        boom.Throw(throwPower);
    }

    private void RayCheck()
    {
        RaycastHit hit;
        if (xri.TryGetCurrent3DRaycastHit(out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }

   
}
