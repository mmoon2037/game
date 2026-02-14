using UnityEngine;
using TMPro; // Добавляем для работы с текстом

public class EnemyTouch : MonoBehaviour
{
    [Header("Настройки")]
    public string playerTag = "Player";
    
    [Header("UI Смерти")]
    public TextMeshProUGUI deathText; // Перетащите сюда объект текста (например, "YOU DIED")
    public string deathMessage = "ВЫ ПОЙМАНЫ";

    private void Start()
    {
        // Скрываем текст в начале игры
        if (deathText != null) deathText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // 1. Показываем текст смерти
            if (deathText != null)
            {
                deathText.text = deathMessage;
                deathText.gameObject.SetActive(true);
            }

            // 2. Запускаем затемнение
            ScreenFader fader = FindObjectOfType<ScreenFader>();
            if (fader != null)
            {
                fader.StartFade();
            }

            // 3. Останавливаем врага
            if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent))
            {
                agent.isStopped = true;
            }
            
            // 4. Отключаем скрипты логики (патруль, погоня, таунт)
            if (TryGetComponent<EnemyAI>(out var ai)) ai.enabled = false;
            if (TryGetComponent<EnemyChase>(out var chase)) chase.enabled = false;
            if (TryGetComponent<EnemyTaunt>(out var taunt)) taunt.enabled = false;
        }
    }
}
