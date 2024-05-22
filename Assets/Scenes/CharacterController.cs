using UnityEngine;

public class GameController : MonoBehaviour
{
    public float moveSpeed = 5f;  // ĳ���� �̵� �ӵ�
    public Vector3 cameraOffset;  // ī�޶�� �÷��̾� ������ ������
    public float rotationSpeed = 100f;  // ���콺 ȸ�� �ӵ�

    private Rigidbody playerRb;
    private Transform playerTransform;
    private Camera mainCamera;
    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        // Player�� Camera�� ã��
        playerRb = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // ĳ���� �̵� ó��
        HandleMovement();

        // ī�޶� ���� ó��
        HandleCameraFollow();

        // ���콺 ȸ�� ó��
        HandleMouseRotation();
    }

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 moveDirection = mainCamera.transform.TransformDirection(movement);
        moveDirection.y = 0f; // Y�� �������� ����

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
            pitch = Mathf.Clamp(pitch, -89f, 89f); // ��ġ ������ �����Ͽ� ī�޶� �������� �ʵ��� ��

            mainCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            mainCamera.transform.position = playerTransform.position + cameraOffset;
        
    }
}
