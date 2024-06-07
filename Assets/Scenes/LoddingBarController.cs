using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoddingBarController : MonoBehaviour
{
    public delegate void LoddingBarDelegate();
    private LoddingBarDelegate loddingBarCallBack;
    public LoddingBarDelegate LoddingBarCallBack
    {
        set { loddingBarCallBack = value; }
    }

    [SerializeField] Image LoddingBar;

    private bool isDone = false;
    private float progress = 0f;
    public bool IsDone { get { return isDone; } set { isDone = value; } }
    public float Progress { get { return progress; } set { progress = value; } }


    public void StartLodding()
    {
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        Debug.Log("Coroutine Active");
        loddingBarCallBack?.Invoke();
        LoddingBar.fillAmount = 0f;
        while (!isDone)
        {
            yield return null;

            LoddingBar.fillAmount = Mathf.Lerp(LoddingBar.fillAmount, progress / 100f, Time.deltaTime * 0.1f); // progress를 100으로 나눠서 0-1 범위로 변환
        }

        // isDone 상태가 되면 로딩 바가 100%가 될 때까지 기다린다
        while (LoddingBar.fillAmount < 0.95f)
        {
            LoddingBar.fillAmount = Mathf.Lerp(LoddingBar.fillAmount, 1f, Time.deltaTime); // 100%로 설정
            yield return null;
        }

        LoddingBar.fillAmount = 1f; // 확실히 100%로 설정
        this.gameObject.SetActive(false);
    }
}
