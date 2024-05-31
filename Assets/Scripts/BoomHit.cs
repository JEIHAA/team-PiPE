using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomHit : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        // �浹�� �������� ������ ����
        ContactPoint[] contactPoints = collision.contacts;

        foreach (ContactPoint contact in contactPoints)
        {
            // contact.point�� �浹 ������ ���� ��ǥ�� ��Ÿ���ϴ�.
            Vector3 collisionPoint = contact.point;
            Debug.Log("Collision point: " + collisionPoint);
        }
    }
}
