using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class XRPlayerController : MonoBehaviour
{
    
    private Rigidbody rb;
    private GameObject origin;
    private CapsuleCollider capsuleCollider;
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float protrusionDistance = 0.01f;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float addWeight;
    private float rayDistance = 2f;
    private RaycastHit slopeHit;
    private int groundLayer;
    private bool isSlope;
    private bool isGrounded;
    private Vector3 direction;
    private PhotonView photonView;
    RaycastHit spherCasthit;

    [SerializeField] private float maxSlopeAngle = 30f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundLayer = LayerMask.GetMask("Ground");
        origin = GameObject.FindGameObjectWithTag("XROrigin");
        capsuleCollider = GetComponent<CapsuleCollider>();
        photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        rb.freezeRotation = true;
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
        Vector3 gravity = Vector3.down * Mathf.Abs(rb.velocity.y);
        isGrounded = IsGrounded();
        isSlope = OnSlope();

        Debug.Log("isGrounded : " + isGrounded);
        Debug.Log("isSlope : " + isSlope);
        if (isGrounded && isSlope)
        {
            velocity = AdjustDirectionToSlope(direction);
            gravity = Vector3.zero;
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
        currentMoveSpeed = moveSpeed - addWeight;
        rb.velocity = velocity * currentMoveSpeed/* + gravity*/;
    }

    private bool OnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out slopeHit, rayDistance, groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < maxSlopeAngle;
        }
        return false;
    }

    private Vector3 AdjustDirectionToSlope(Vector3 slopeDir)
    {
        return Vector3.ProjectOnPlane(slopeDir, slopeHit.normal).normalized;
    }

    private bool IsGrounded()
    {
        float sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        float sphereRadius = sphereScale / 3;
        float sphereCastDistance = (capsuleCollider.height / 2) + protrusionDistance - sphereRadius;
        return Physics.SphereCast(transform.position, sphereRadius, -transform.up, out spherCasthit, sphereCastDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        Gizmos.DrawWireSphere(transform.position + (-transform.up) * spherCasthit.distance, sphereScale / 3);
    }
}
