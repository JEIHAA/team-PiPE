using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//마우스 방향 컨트롤

public class mouseLook : MonoBehaviour
{

    [SerializeField] float mouseScensitivity = 100;
    [SerializeField] Transform cameraTrans = null;

    float xRotate = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

  
    void FixedUpdate()
    {
        Vector3 mouse = Vector3.zero;
        mouse.x = Input.GetAxis("Mouse X");
        mouse.y = Input.GetAxis("Mouse Y");

        mouse *= mouseScensitivity * Time.fixedDeltaTime; //이동 프레임 연산식


        xRotate -= mouse.y;
        xRotate = Mathf.Clamp(xRotate, -90, 90);

        cameraTrans.localRotation = Quaternion.Euler(xRotate, 0, 0);
        transform.Rotate(Vector3.up * mouse.x);
    }
}
