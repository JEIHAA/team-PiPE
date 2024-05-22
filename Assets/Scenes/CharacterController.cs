using UnityEngine;

public class GameController : MonoBehaviour
{
    public float moveSpeed = 5f;  // 캐릭터 이동 속도
    public Vector3 cameraOffset;  // 카메라와 플레이어 사이의 오프셋
    public float rotationSpeed = 100f;  // 마우스 회전 속도

    private Rigidbody playerRb;
    private Transform playerTransform;
    private Camera mainCamera;
    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        // Player와 Camera를 찾음
        playerRb = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 캐릭터 이동 처리
        HandleMovement();

        // 카메라 추적 처리
        HandleCameraFollow();

        // 마우스 회전 처리
        HandleMouseRotation();
    }

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 moveDirection = mainCamera.transform.TransformDirection(movement);
        moveDirection.y = 0f; // Y축 움직임을 무시

        playerRb.velocity = moveDirection * moveSpeed;
    }

    void HandleCameraFollow()
    {
        if (mainCamera != null && playerTransform != null)
        {
            mainCamera.transform.position = playerTransform.position + cameraOffset;
        }
    }

    void HandleMouseRotation()
    { 
            yaw += rotationSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
            pitch -= rotationSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, -89f, 89f); // 피치 각도를 제한하여 카메라가 뒤집히지 않도록 함

            mainCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            mainCamera.transform.position = playerTransform.position + cameraOffset;
        
    }
}
