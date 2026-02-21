using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyTouch : MonoBehaviour
{
    [Header("Настройки")]
    public string playerTag = "Player";
    public string menuSceneName = "Menu"; // Исправил на "Menu"

    [Header("UI Элементы")]
    public TextMeshProUGUI deathText;
    public string deathMessage = "ВЫ ПОЙМАНЫ";
    public GameObject deathMenu;
    public CanvasGroup fadePanel;

    [Header("Кнопки")]
    public Button retryButton;
    public Button menuButton;

    [Header("Параметры эффектов")]
    public float fadeSpeed = 0.8f;

    private bool isDead = false;

    private void Start()
    {
        // Скрываем всё при старте
        if (deathText != null) deathText.gameObject.SetActive(false);
        if (deathMenu != null) deathMenu.gameObject.SetActive(false);

        // Скрываем кнопки персонально (на всякий случай)
        if (retryButton != null) retryButton.gameObject.SetActive(false);
        if (menuButton != null) menuButton.gameObject.SetActive(false);

        if (fadePanel != null)
        {
            fadePanel.alpha = 0;
            fadePanel.blocksRaycasts = false;
        }

        // Назначаем функции
        if (retryButton != null) retryButton.onClick.AddListener(RestartLevel);
        if (menuButton != null) menuButton.onClick.AddListener(GoToMenu);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !isDead)
        {
            isDead = true;
            StartCoroutine(GameOverSequence());
        }
    }

    IEnumerator GameOverSequence()
    {
        if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent))
            agent.isStopped = true;

        DisableEnemyLogic();

        if (deathText != null)
        {
            deathText.text = deathMessage;
            deathText.gameObject.SetActive(true);
        }

        if (fadePanel != null)
        {
            fadePanel.blocksRaycasts = true;
            while (fadePanel.alpha < 1)
            {
                fadePanel.alpha += Time.unscaledDeltaTime * fadeSpeed;
                yield return null;
            }
        }

        // ВКЛЮЧАЕМ МЕНЮ И КНОПКИ
        if (deathMenu != null) deathMenu.SetActive(true);
        if (retryButton != null) retryButton.gameObject.SetActive(true);
        if (menuButton != null) menuButton.gameObject.SetActive(true);

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
        // Используем переменную menuSceneName
        SceneManager.LoadScene(menuSceneName);
    }

    private void DisableEnemyLogic()
    {
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var s in scripts)
        {
            string n = s.GetType().Name.ToLower();
            if (s != this && (n.Contains("enemy") || n.Contains("ai") || n.Contains("chase")))
                s.enabled = false;
        }
    }
}
