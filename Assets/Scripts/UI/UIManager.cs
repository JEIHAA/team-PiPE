using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool isPC;
    [SerializeField] private GameObject PCUI;
    [SerializeField] private GameObject VRUI;
    [SerializeField] private LoddingBarController loddingbar;

    private void Awake()
    {
        loddingbar.LoddingBarCallBack = CoroutineActive;
    }
    public void LazyAwake()
    {
        if (isPC)
        {
            PCUI.SetActive(true);
            VRUI.SetActive(false);
        }
        else
        {
            PCUI.SetActive(false);
            VRUI.SetActive(true);
        }
    }

    public void LoddingStatus(bool _isDone, float _progress)
    {
        Debug.Log("UIM, LoddingStatus: " + _isDone + " " + _progress);
        loddingbar.IsDone = _isDone;
        loddingbar.Progress = _progress;
    }

    public void loddingActive()
    {
        loddingbar.StartLodding();
    }

    private void CoroutineActive()
    {
        Debug.Log("CallBack Active");
        GameManager.Instance().IsFinished = true;
    }
}
