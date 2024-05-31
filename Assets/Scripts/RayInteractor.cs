using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
public class RayInteractor : MonoBehaviourPunCallbacks
{
    public XRRayInteractor xri;
    
    

    private void Update()
    {
        RayCheck();
    }

    private void SelectEvent(SelectEnterEventArgs args)
    {
        //boom.XRGrab();
    }

    private void SeletExit(SelectExitEventArgs args)
    {
        //boom.XRRealease();
        //boom.Throw(rightCon, throwPower);
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
