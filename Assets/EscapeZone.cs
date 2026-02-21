using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EscapeZone : MonoBehaviour
{
    [Header("Настройки")]
    public string menuSceneName = "Menu"; // Имя сцены с меню

    [Header("UI Элементы")]
    public TextMeshProUGUI infoText;
    public string winMessage = "ТЫ СБЕЖАЛ!";
    public GameObject winMenu;      // Панель победы (можно использовать ту же, что для смерти)
    public CanvasGroup fadePanel;   // Затемнение

    [Header("Кнопки")]
    public Button retryButton;
    public Button menuButton;

    [Header("Параметры эффектов")]
    public float fadeSpeed = 0.8f;

    private bool canEscape = false;

    void Awake()
    {
        // Скрываем всё при старте
        if (infoText != null) infoText.gameObject.SetActive(false);
        if (winMenu != null) winMenu.gameObject.SetActive(false);
        if (fadePanel != null) fadePanel.alpha = 0;

        // Подключаем кнопки
        if (retryButton != null) retryButton.onClick.AddListener(RestartLevel);
        if (menuButton != null) menuButton.onClick.AddListener(GoToMenu);
    }

    public void ActivateEscape()
    {
        canEscape = true;
        // gameObject.SetActive(true); // Включаем зону, если она была выключена
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canEscape)
        {
            StartCoroutine(WinSequence());
        }
    }

    IEnumerator WinSequence()
    {
        // 1. Текст победы
        if (infoText != null)
        {
            infoText.text = winMessage;
            infoText.gameObject.SetActive(true);
        }

        // 2. Затемнение
        if (fadePanel != null)
        {
            fadePanel.blocksRaycasts = true;
            while (fadePanel.alpha < 1)
            {
                fadePanel.alpha += Time.unscaledDeltaTime * fadeSpeed;
                yield return null;
            }
        }

        // 3. Показываем меню и кнопки
        if (winMenu != null) winMenu.SetActive(true);
        if (retryButton != null) retryButton.gameObject.SetActive(true);
        if (menuButton != null) menuButton.gameObject.SetActive(true);

        // 4. Курсор и пауза
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
