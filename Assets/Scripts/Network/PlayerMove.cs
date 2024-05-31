using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMove : MonoBehaviourPun
{
    public float moveSpeed = 3.0f;
    public float rotSpeed = 200.0f;
    public GameObject XROrigin;
    public Transform Charactor;
    public InputActionAsset InputAction;

    private void Start()
    {
        
    }

    private void Update()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        Vector2 stickPos = InputAction.actionMaps[3].actions[5].ReadValue<Vector2>();

        Vector3 dir = new Vector3(stickPos.x, 0, stickPos.y);
        dir.Normalize();

        dir = XROrigin.transform.TransformDirection(dir);
        transform.position += dir * moveSpeed * Time.deltaTime;

        float magnitude = dir.magnitude;

        if (magnitude > 0)
        {
            Charactor.rotation = Quaternion.LookRotation(dir);
        }

    }

    private void Turn()
    {
        float rotH = InputAction.actionMaps[6].actions[4].ReadValue<Vector2>().x;

        XROrigin.transform.eulerAngles += new Vector3(0, rotH, 0) * rotSpeed * Time.deltaTime;
    }
}
