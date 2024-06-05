using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UserInterface : MonoBehaviour
{
    public GameObject PC_UserInterface;

    public void Start()
    {
        PC_UserInterface.SetActive(false);       
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PC_UserInterface.SetActive(true);
            Cursor.lockState = CursorLockMode.None; //Tab눌럿을때 커서 자유롭게
            Cursor.visible = true;
        }
    }

    public void PCGameSceneCtrl()
    {
        SceneManager.LoadScene("Scenes/PC/Lobby"); //이동할 씬 이름 입력 수정가능
        Debug.Log("Lobby로 이동하였습니다");
    }

    public void BackButton()
    {
        PC_UserInterface.SetActive(false);
        Cursor.visible = false;
    }

    

}


