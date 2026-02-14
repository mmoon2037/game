using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public float fadeSpeed = 0.5f; // Скорость затемнения
    private Image fadeImage;
    private bool isFading = false;

    void Awake()
    {
        fadeImage = GetComponent<Image>();
        // Устанавливаем начальную прозрачность на 0
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.raycastTarget = false; // Чтобы не мешал кликать в меню, пока прозрачный
    }

    public void StartFade()
    {
        if (!isFading)
        {
            fadeImage.raycastTarget = true; 
            StartCoroutine(FadeToBlack());
        }
    }

    private IEnumerator FadeToBlack()
    {
        isFading = true;
        float alpha = 0;

        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}
