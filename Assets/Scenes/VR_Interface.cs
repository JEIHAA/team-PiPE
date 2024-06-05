using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VR_Interface : MonoBehaviour
{
    public GameObject VR_UserInterface;
    public GameObject PanelMinimap;
    public GameObject LobbyButton;

    public void Start()
    {
        VR_UserInterface.SetActive(false);
        PanelMinimap.SetActive(false);
        LobbyButton.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            VR_UserInterface.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void VRGameSceneCtl()
    {
        SceneManager.LoadScene("Scenes/XR/Lobby"); //이동할 씬 이름 입력 수정가능
        Debug.Log("Lobby로 이동하였습니다");
    }

    public void VR_BackButton()
    {
        VR_UserInterface.SetActive(false);
        Cursor.visible = false;
    }

    public void VR_MapButton()
    {
        if (PanelMinimap != null)
        {
            PanelMinimap.SetActive(true);
        }
        else
        {
            Debug.LogError("PanelMinimap is not assigned");
        }
        if (LobbyButton != null)
        {
            LobbyButton.SetActive(false);
        }
        else
        {
            Debug.LogError("LobbyButtonis not assigned");
        }
    }

    public void VR_SettingButton()
    {
        if (PanelMinimap != null)
        {
            PanelMinimap.SetActive(false);
        }
        else
        {
            Debug.LogError("PanelMinimap is not assigned");
        }
        if (LobbyButton != null)
        {
            LobbyButton.SetActive(true);
        }
        else
        {
            Debug.LogError("LobbyButtonis not assigned");
        }
    }
}
