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
            Cursor.lockState = CursorLockMode.None; //Tab�������� Ŀ�� �����Ӱ�
            Cursor.visible = true;
        }
    }

    public void PCGameSceneCtrl()
    {
        SceneManager.LoadScene("Scenes/PC/Lobby"); //�̵��� �� �̸� �Է� ��������
        Debug.Log("Lobby�� �̵��Ͽ����ϴ�");
    }

    public void BackButton()
    {
        PC_UserInterface.SetActive(false);
        Cursor.visible = false;
    }

    

}


