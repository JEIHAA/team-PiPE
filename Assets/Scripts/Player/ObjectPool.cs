using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ObjectPool : MonoBehaviour
{
    private static ObjectPool ObjectPoolInstace;
    private Queue<XRGrabInterAction> poolingObjectQueue = new Queue<XRGrabInterAction>();

    private void Awake()
    {
        // ó���� �����ִ� ������Ʈ 10�� ����
        if (PhotonNetwork.IsMasterClient)
        {
            OPInstance();
            ObjectPoolInstace = this;

            StartInit();
        }
    }

    private void StartInit()
    {
        Initialize(10);
    }

    public static ObjectPool OPInstance()
    {
        if(ObjectPoolInstace == null)
        {
            ObjectPoolInstace = new ObjectPool();
        }
        return ObjectPoolInstace;
    }

    private XRGrabInterAction CreateNewObject()
    {
        Debug.Log("Attempting to create new object.");

        // Photon�� ����Ǿ� �ִ��� Ȯ��
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Photon is not connected. Cannot instantiate objects.");
            return null;
        }

        GameObject newObj = PhotonNetwork.Instantiate("Boom", transform.position, transform.rotation);
        newObj.transform.parent = this.transform;

        if (newObj == null)
        {
            Debug.LogError("Failed to instantiate object with PhotonNetwork.Instantiate.");
            return null;
        }

        XRGrabInterAction newBomb = newObj.GetComponent<XRGrabInterAction>();

        if (newBomb == null)
        {
            Debug.LogError("The instantiated object does not have an XRGrabInterAction component.");
            return null;
        }

        newObj.SetActive(false);
        return newBomb;
    }

    private void Initialize(int _count)
    {
        for (int i = 0; i < _count; ++i)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    // ������Ʈ �����ִ� �Լ�
    public static XRGrabInterAction GetObject()
    {
        if(ObjectPoolInstace.poolingObjectQueue.Count > 0)
        {
            XRGrabInterAction obj = ObjectPoolInstace.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            XRGrabInterAction newObj = ObjectPoolInstace.CreateNewObject();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    // ������ ������Ʈ �ٽ� ȸ���ϴ� �Լ�
    public static void ReturnObject(XRGrabInterAction _bomb)
    {
        _bomb.gameObject.SetActive(false);
        _bomb.transform.SetParent(ObjectPoolInstace.gameObject.transform);
        ObjectPoolInstace.poolingObjectQueue.Enqueue(_bomb);
        Debug.Log("Return Obecjt :" + ObjectPoolInstace.poolingObjectQueue.Count);
    }
}
