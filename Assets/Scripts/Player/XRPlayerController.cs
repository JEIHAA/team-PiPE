using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class XRPlayerController : MonoBehaviour
{

    private CharacterController xrCC;
    private GameObject origin;
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float protrusionDistance = 0.05f;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float addWeight;
    [SerializeField] private float gravitationalAcceleration;
    private int playerID;
    private float rayDistance;
    private RaycastHit slopeHit;
    private int groundLayer;
    private bool isSlope;
    private bool isGrounded;
    private Vector3 direction;
    private PhotonView photonView;
    RaycastHit spherCasthit;


    private void Awake()
    {
        playerID = PhotonNetwork.LocalPlayer.ActorNumber;
        xrCC = GetComponent<CharacterController>();
        groundLayer = LayerMask.GetMask("Ground");
        origin = GameObject.FindGameObjectWithTag("XROrigin");
        photonView = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            Move();
            origin.transform.position = transform.position;
            transform.rotation = origin.transform.rotation;
        }
        
    }



    protected void Move()
    {
        float h = actionAsset.actionMaps[3].actions[5].ReadValue<Vector2>().x;
        float v = actionAsset.actionMaps[3].actions[5].ReadValue<Vector2>().y;
        Vector3 forward = transform.forward * v;
        Vector3 right = transform.right * h;
        direction = forward + right;
        Vector3 velocity = direction;
        isGrounded = IsGrounded();
        isSlope = OnSlope();

        if (isSlope)
        {
            velocity = AdjustDirectionToSlope(direction);
        }
        if (!isGrounded)
        {
            velocity.y = velocity.y - gravitationalAcceleration * Time.fixedDeltaTime;
        }
        
       
        currentMoveSpeed = moveSpeed - addWeight;
        xrCC.Move(velocity * currentMoveSpeed * Time.deltaTime);
    }

    private bool OnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out slopeHit, Mathf.Infinity, groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < xrCC.slopeLimit;
        }
        return false;
    }
    public int SendId()
    {
        return playerID;
    }


    private Vector3 AdjustDirectionToSlope(Vector3 slopeDir)
    {
        return Vector3.ProjectOnPlane(slopeDir, slopeHit.normal).normalized;
    }

    private bool IsGrounded()
    {
        float sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        float sphereRadius = sphereScale / 3;
        float sphereCastDistance = (xrCC.height / 2) + protrusionDistance - sphereRadius;
        return Physics.SphereCast(transform.position, sphereRadius, -transform.up, out spherCasthit, sphereCastDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        Gizmos.DrawWireSphere(transform.position + (-transform.up) * spherCasthit.distance, sphereScale / 3);
    }
}
