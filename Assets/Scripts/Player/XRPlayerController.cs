using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRPlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] protected float moveSpeed;
    [SerializeField] private float currentMoveSpeed;

    [SerializeField] private float addWeight;
    [SerializeField] private float mouseSpeed;
    [SerializeField] private int jumpHeight;
    [SerializeField] private float jumpForce = 6f;
    private float rayDistance = 2f;
    private RaycastHit slopeHit;
    private int groundLayer;
    private bool isSlope;
    private bool isGrounded;
    private Vector3 direction;
    RaycastHit spherCasthit;
    private PhotonView photonView;

    [SerializeField] private float maxSlopeAngle = 30f;





    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundLayer = LayerMask.GetMask("Ground");
        photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서를 화면 안에서 고정
        Cursor.visible = false;
        rb.freezeRotation = true;
    }



    private void FixedUpdate()
    {
        
        if(photonView.IsMine)
            Move();

    }




    protected void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 forward = transform.forward * v;
        Vector3 right = transform.right * h;
        direction = forward + right;
        Vector3 velocity = direction;
        Vector3 gravity = Vector3.down * Mathf.Abs(rb.velocity.y);
        isGrounded = IsGrounded();
        isSlope = OnSlope();

        Debug.Log("isGrounded : " + isGrounded);
        Debug.Log("isSlope : " + isSlope);
        if (isGrounded)
        {
            velocity = AdjustDirectionToSlope(direction);
            gravity = Vector3.zero;
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
        Debug.Log("Gravity : " + gravity);
        currentMoveSpeed = moveSpeed - addWeight;
        rb.velocity = velocity * currentMoveSpeed + gravity;
    }



    /*private void Jump()
    {
        Debug.Log(isGrounded);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }*/

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
        Debug.Log(Vector3.ProjectOnPlane(slopeDir, slopeHit.normal).normalized);
        return Vector3.ProjectOnPlane(slopeDir, slopeHit.normal).normalized;
    }

    private bool IsGrounded()
    {
        float sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        Debug.Log(sphereScale + "  :   " + sphereScale / 3);
        return Physics.SphereCast(transform.position, sphereScale / 3, -transform.up, out spherCasthit, sphereScale - 0.31f, groundLayer); ;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        Gizmos.DrawWireSphere(transform.position + (-transform.up) * spherCasthit.distance, sphereScale / 3);
    }
}
