using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    private static FadeOut instance;

    public CanvasGroup fadeBg;

    // Fade 처리 시간
    [Range(0.5f, 2.0f)]
    public float fadeDuration = 1.0f;

    public static FadeOut MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FadeOut>();
            }

            return instance;
        }
    }

    public IEnumerator Fade(float finalAlpha)
    {
        fadeBg.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(fadeBg.alpha - finalAlpha) / fadeDuration;

        while (!Mathf.Approximately(fadeBg.alpha, finalAlpha))
        {
            fadeBg.alpha = Mathf.MoveTowards(fadeBg.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

            yield return null;
        }

        fadeBg.blocksRaycasts = false;
    }
}
