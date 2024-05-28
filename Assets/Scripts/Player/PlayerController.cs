using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Camera maincam;
    [SerializeField] protected float moveSpeed;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float maxSlopeAngle = 30f;
    [SerializeField] private float addWeight;
    [SerializeField] private float mouseSpeed;
    private float yRotation;
    private float xRotation;
    private float rayDistance = 2f;
    private RaycastHit slopeHit;
    private int groundLayer;
    private bool isSlope;
    private bool isGrounded;
    private Vector3 direction;
    private PhotonView photonView;
    RaycastHit spherCasthit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        maincam = GetComponentInChildren<Camera>();
        groundLayer = LayerMask.GetMask("Ground");
        photonView = GetComponent<PhotonView>();

    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서를 화면 안에서 고정
        Cursor.visible = false;
        rb.freezeRotation = true;

    }

 
    private void Update()
    {
       if (photonView.IsMine)
        {
            Rotate();
            Move();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        
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

        if(isGrounded)
        {
            velocity = AdjustDirectionToSlope(direction);
            gravity = Vector3.zero;
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
            rb.AddForce(Vector3.down * 200f, ForceMode.Force);
        }
        Debug.Log("Gravity : "+ gravity);
        currentMoveSpeed = moveSpeed - addWeight;
        rb.velocity = velocity * currentMoveSpeed;
    }


    private void Rotate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;
        yRotation += mouseX;    // 마우스 X축 입력에 따라 수평 회전 값을 조정
        xRotation -= mouseY;    // 마우스 Y축 입력에 따라 수직 회전 값을 조정

        xRotation = Mathf.Clamp(xRotation, -50f, 70f);  // 수직 회전 값을 -90도에서 90도 사이로 제한

        maincam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); // 카메라의 회전을 조절
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up*60f, ForceMode.Impulse);
    }
    

    private bool OnSlope()
    { 
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out slopeHit , rayDistance, groundLayer))
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
        return Physics.SphereCast(transform.position, sphereScale/3, -transform.up, out spherCasthit, sphereScale-0.31f, groundLayer); ;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        Gizmos.DrawWireSphere(transform.position+ (-transform.up) * spherCasthit.distance, sphereScale/3);
    }

}
