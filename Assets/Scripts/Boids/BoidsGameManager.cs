using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsGameManager : MonoBehaviour
{
  [SerializeField] private BoidsPlayerManager[] pm;

  /*private void FixedUpdate()
  {
    GetPlayerHasTotalBoids();
  }*/

  private int GetPlayerHasTotalBoids() {
    int totalBoids = 0;
    for (int i = 0; i < pm.Length; i++)
    {
      totalBoids += pm[i].GetHasBoidsNum();
    }
    Debug.Log($"totalBoids: {totalBoids}");
    return totalBoids;
  }
}
