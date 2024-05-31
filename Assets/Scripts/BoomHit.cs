using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomHit : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        // 충돌한 지점들의 정보를 저장
        ContactPoint[] contactPoints = collision.contacts;

        foreach (ContactPoint contact in contactPoints)
        {
            // contact.point는 충돌 지점의 월드 좌표를 나타냅니다.
            Vector3 collisionPoint = contact.point;
            Debug.Log("Collision point: " + collisionPoint);
        }
    }
}
