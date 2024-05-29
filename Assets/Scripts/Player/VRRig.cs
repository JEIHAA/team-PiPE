using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class VRRig : MonoBehaviourPunCallbacks
{
    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;

    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public PhotonView PV;
    public GameObject vrPlayer;
    public Transform headConstraint;
    public Vector3 headBodyOffset;
    public float turnSmoothness = 3f;

    private void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
        ActionBasedController[] controller = FindObjectsOfType<ActionBasedController>();

        head.vrTarget = Camera.main.transform;
        leftHand.vrTarget = controller[0].transform;
        rightHand.vrTarget = controller[1].transform;
    }
    private void Update()
    {
        if (PV.IsMine)
        {
            vrPlayer.SetActive(false);

            transform.position = headConstraint.position + headBodyOffset;
            transform.forward = Vector3.Lerp(transform.forward,
            Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized, Time.deltaTime * turnSmoothness);

            head.Map();
            leftHand.Map();
            rightHand.Map();
        }
    }
}