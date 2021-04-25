using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private static string nextScene;
    [SerializeField] Image loagdingBar;

    void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;

        while (!op.isDone)
        {

            yield return null;

            timer += Time.deltaTime;

            if (op.progress < 0.9f)
            {
                loagdingBar.fillAmount = Mathf.Lerp(loagdingBar.fillAmount, op.progress, timer);

                if (loagdingBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }

            else
            {
                loagdingBar.fillAmount = Mathf.Lerp(loagdingBar.fillAmount, 1f, timer);

                if (loagdingBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;

                    yield break;
                }
            }
        }
    }
}
