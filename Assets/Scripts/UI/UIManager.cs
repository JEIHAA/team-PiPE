using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool isPC;
    [SerializeField] private GameObject PCUI;
    [SerializeField] private GameObject VRUI;

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

    public void SetCurClient(bool _status)
    {
        isPC = _status;
    }
}
