using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PCPlayerController : MonoBehaviourPun
{
    private CharacterController pcCC;
    private Camera maincam;
    private GameObject pcOrigin;
    [SerializeField] private Transform myHeadPos;
    [SerializeField] private GameObject head;
    [SerializeField] protected float moveSpeed;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float maxSlopeAngle = 30f;
    [SerializeField] private float protrusionDistance = 0.05f;
    [SerializeField] private float addWeight;
    [SerializeField] private float mouseSpeed;
    [SerializeField] private float gravitationalAcceleration;
    private int playerID;
    private float yRotation;
    private float xRotation;
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
        pcOrigin = GameObject.FindGameObjectWithTag("PCOrigin");
        maincam = pcOrigin.GetComponentInChildren<Camera>();
        groundLayer = LayerMask.GetMask("Ground");
        pcCC = GetComponentInChildren<CharacterController>();
        photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서를 화면 안에서 고정
        Cursor.visible = false;
    }

 
    private void Update()
  {
    pcOrigin.transform.position = myHeadPos.transform.position;
    head.transform.localScale = Vector3.zero;
    Rotate();
    Move();
    /* if (photonView.IsMine)
     {

     }*/


  }


  protected void Move()
    {
    Debug.Log("ㅇㅇㅇㅇㅇ");
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
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
        pcCC.Move(velocity * currentMoveSpeed * Time.deltaTime);
    }


    private void Rotate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;
        yRotation += mouseX;    // 마우스 X축 입력에 따라 수평 회전 값을 조정
        xRotation -= mouseY;    // 마우스 Y축 입력에 따라 수직 회전 값을 조정

        xRotation = Mathf.Clamp(xRotation, -35f, 70f);  // 수직 회전 값을 -35도에서 70도 사이로 제한

        maincam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); // 카메라의 회전을 조절
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }


    private bool OnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out slopeHit, Mathf.Infinity, groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < pcCC.slopeLimit;
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
        float sphereCastDistance = (pcCC.height / 2) + protrusionDistance - sphereRadius;
        Vector3 castPos = transform.position;
        castPos.y = transform.position.y + pcCC.center.y;
        return Physics.SphereCast(castPos, sphereRadius, -transform.up, out spherCasthit, sphereCastDistance, groundLayer);
    }

    public int SendId()
    {
        return playerID;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        Gizmos.DrawWireSphere(transform.position+ (-transform.up) * spherCasthit.distance, sphereScale/3);
    }

}
