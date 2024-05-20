using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public static List<GameObject> collidObject = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.gameObject.layer == 6) return;
        collidObject.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        collidObject.Remove(other.gameObject);
    }
}
