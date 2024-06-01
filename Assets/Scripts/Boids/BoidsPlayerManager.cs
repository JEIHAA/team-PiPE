using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsPlayerManager : MonoBehaviour
{
  private Queue<GameObject> assignBoidQueue = new Queue<GameObject>();
  public Queue<GameObject> AssignBoidQueue { get { return assignBoidQueue; } set { assignBoidQueue = value; } }

  private void FixedUpdate()
  {
    GetHasBoidsNum();
  }

  public int GetHasBoidsNum() {
    Debug.Log($"{this.gameObject.name}'s has boid: {assignBoidQueue.Count}");
    return assignBoidQueue.Count;
  }
}
