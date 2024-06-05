using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargingUI : MonoBehaviour
{

    public Image progressBar;
    public GameObject CharjingBar;
    public float chargeSpeed = 1f;
    private bool isCharging = false;
    private float currentCharge = 0f;


    public void Start()
    {
        CharjingBar.active = false;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            CharjingBar.active = true;
            isCharging = true;
        }


        if (Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            currentCharge = 0f; // 게이지 초기화
            UpdateProgressBar();
            CharjingBar.active = false;
        }

        // 충전 중일 때 게이지 증가
        if (isCharging)
        {
            ObjectCharging();
        }

    }

    public void ObjectCharging()
    {
        currentCharge += Time.deltaTime * chargeSpeed;
        currentCharge = Mathf.Clamp(currentCharge, 0f, 1f);
        UpdateProgressBar();
    }
    public void UpdateProgressBar()
    {
        progressBar.fillAmount = currentCharge;
    }
}