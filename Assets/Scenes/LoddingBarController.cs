using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoddingBarController : MonoBehaviour
{
    static string Main_TestScene;
    [SerializeField] Image LoddingBar;

    public static void LoadScene(string sceneName)
    {
        Main_TestScene = sceneName;
        SceneManager.LoadScene("Lodding");
    }
    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(Main_TestScene);
        op.allowSceneActivation = false; //�ε� ���� �����ϸ鼭 ���� �� ������Ʈ�� �ҷ����� �۾��� ����

        float Timer = 0f;
        while(!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                LoddingBar.fillAmount = op.progress;
            }
            else
            {
                Timer += Time.unscaledDeltaTime;
                LoddingBar.fillAmount = Mathf.Lerp(0.9f, 1f, Timer);
                if(LoddingBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
