using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoUI : MonoBehaviour
{
    [SerializeField]
    private GameObject photo;
    [SerializeField]
    private GameObject HoldingPosPicture;
    [SerializeField]
    private GameObject TakingPosPicture;
    [SerializeField]
    private GameObject viewFinder;

    public GameObject GetPhoto()
    {
        return photo;
    }
    public GameObject GetHoldingPos()
    {
        return HoldingPosPicture;
    }
    public GameObject GetTakingPos()
    {
        return TakingPosPicture;
    }
    public GameObject GetViewFinder()
    {
        return viewFinder;
    }

}
