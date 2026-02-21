using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public CanvasGroup fadePanel; // Тот самый Image с компонентом CanvasGroup
    public float fadeSpeed = 0.8f;

    void Awake()
    {
        if (fadePanel != null) fadePanel.alpha = 0;
    }

    public void StartFade()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        while (fadePanel.alpha < 1)
        {
            // Используем yield return null, чтобы работало, даже если Time.timeScale = 0
            fadePanel.alpha += Time.unscaledDeltaTime * fadeSpeed;
            yield return null;
        }
    }
}
