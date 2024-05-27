using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryObject : MonoBehaviour
{
    public bool isPc { get; private set; }
    [SerializeField] bool SetAsPC = false;
    private void Awake()
    {
        var obj = FindObjectsOfType<DontDestoryObject>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        isPc = SetAsPC;
    }
}
