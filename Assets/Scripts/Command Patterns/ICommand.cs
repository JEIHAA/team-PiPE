using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    void Execute();
}

public class CommandInvoker
{

    public static void ExecuteCommand(ICommand command)
    {
        command.Execute();
    }
}