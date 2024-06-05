using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLodeTester : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoddingBarController.LoadScene("Main_Test");
        }
    }
}
