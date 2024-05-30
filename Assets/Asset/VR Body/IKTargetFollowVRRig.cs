using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public void Map()
    {
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class IKTargetFollowVRRig : MonoBehaviourPun
{
    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;

    //public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public PhotonView PV;
    public GameObject vrPlayer;
    public Transform headConstraint;
    public Vector3 headBodyOffset;
    public float turnSmoothness = 3f;

    public GameObject myHead;

    private void Awake()
    {

    }
    private void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
        GameObject[] controller = GameObject.FindGameObjectsWithTag("Controller");

        //head.vrTarget = Camera.main.transform;
        leftHand.vrTarget = controller[1].transform;
        rightHand.vrTarget = controller[0].transform;
    }

    private void FixedUpdate()
    {
        if (PV.IsMine)
        {
            myHead.transform.localScale = Vector3.zero;

            //transform.position = headConstraint.position + headBodyOffset;
            transform.forward = Vector3.Lerp(transform.forward,
            Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized, Time.deltaTime * turnSmoothness);

            //head.Map();
            leftHand.Map();
            rightHand.Map();
        }

        /*transform.position = headConstraint.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward,
        Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized, Time.deltaTime * turnSmoothness);

        head.Map();
        leftHand.Map();
        rightHand.Map();*/
    }
}

