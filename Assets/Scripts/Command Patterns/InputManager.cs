using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerMover player;
    private float delta_time = 0f;

    private void Start()
    {
        delta_time = Time.deltaTime;
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            RunPlayerCommand(player, Vector3.forward);
        }

        if (Input.GetKey(KeyCode.A))
        {
            RunPlayerCommand(player, Vector3.left);
        }

        if (Input.GetKey(KeyCode.S))
        {
            RunPlayerCommand(player, Vector3.back);
        }

        if (Input.GetKey(KeyCode.D))
        {
            RunPlayerCommand(player, Vector3.right);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            RunPlayerCommand(player, Vector3.up);
        }
    }

    private void RunPlayerCommand(PlayerMover playerMover, Vector3 movement)
    {
        if (playerMover == null)
        {
            return;
        }

        if (playerMover.IsValidMove(movement))
        {
            ICommand command = new MoveCommand(playerMover, movement * delta_time);
            CommandInvoker.ExecuteCommand(command);
        }
    }
}
