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

            LoddingBar.fillAmount = Mathf.Lerp(LoddingBar.fillAmount, progress / 100f, Time.deltaTime * 0.1f); // progress�� 100���� ������ 0-1 ������ ��ȯ
        }

        // isDone ���°� �Ǹ� �ε� �ٰ� 100%�� �� ������ ��ٸ���
        while (LoddingBar.fillAmount < 0.95f)
        {
            LoddingBar.fillAmount = Mathf.Lerp(LoddingBar.fillAmount, 1f, Time.deltaTime); // 100%�� ����
            yield return null;
        }

        LoddingBar.fillAmount = 1f; // Ȯ���� 100%�� ����
        this.gameObject.SetActive(false);
    }
}
