using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoomHit : MonoBehaviour
{
    private PCPlayerController pcController;
    private XRPlayerController xrController;
    DataStruct dataStr;
    public struct DataStruct
    {

        private bool isPerson;
        private int hitPlayerId;

        public int hitPlayerID { get { return hitPlayerId; } set {hitPlayerId = value; } }

        
    }

    private void Awake()
    {
        dataStr = new DataStruct();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 지점들의 정보를 저장
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            ContactPoint[] contactPoints = collision.contacts;
            Debug.Log("       " + contactPoints.Length);

            foreach (ContactPoint contact in contactPoints)
            {
                // contact.point는 충돌 지점의 월드 좌표를 나타냅니다.
                Debug.Log("       " + contact);
                Vector3 collisionPoint = contact.point;
                Debug.Log("Collision point: " + collisionPoint);
            }
        }
        

        if (collision.gameObject.CompareTag("Player"))
        {
            pcController = collision.gameObject.GetComponent<PCPlayerController>();
            xrController = collision.gameObject.GetComponent<XRPlayerController>();

            if(xrController != null )
            {
                dataStr.hitPlayerID = xrController.SendId();
            }
            else if(pcController != null)
            {
                dataStr.hitPlayerID = pcController.SendId();

            }
        }
        Destroy(gameObject);
    }
}
