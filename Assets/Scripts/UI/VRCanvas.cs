using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRCanvas : MonoBehaviour
{
    [SerializeField] Button[] Btns;
    [SerializeField] GameObject[] Tabs;
    private void Awake()
    {
        Debug.Log("Active");
        for (int i = 0; i < Btns.Length; ++i)
        {
            int temp = i;
            Btns[temp].onClick.AddListener(() => ActiveTab(Btns[temp], Tabs[temp]));
        }
        Tabs[1].SetActive(false);
    }

    public void ActiveTab(Button _input, GameObject _target)
    {
        foreach (GameObject go in Tabs)
        {
            go.SetActive(false);
        }

        _target.SetActive(true);
        _input.interactable = false;

        if (_input)
        
            //b.interactable = true;
     
        Debug.Log("Active Tab");
        
    }
}
