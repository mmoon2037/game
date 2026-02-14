using UnityEngine;
using TMPro;
using System.Collections;

public class EscapeZone : MonoBehaviour
{
    [Header("UI Элементы")]
    public TextMeshProUGUI infoText; 
    public string spawnMessage = "ВЫХОД ОТКРЫТ!";
    public string winMessage = "ТЫ СБЕЖАЛ!";
    
    private bool canEscape = false; 

    void Awake()
    {
        
        if (infoText != null) infoText.gameObject.SetActive(false);
    }

    // Метод вызывается из скрипта grab, когда счетчик достигает 15
    public void ActivateEscape()
    {
        canEscape = true;
        gameObject.SetActive(true); // Включаем объект в сцене
        
        if (infoText != null)
        {
            StopAllCoroutines();
            StartCoroutine(ShowNotification(spawnMessage, 4f));
        }
    }

    private IEnumerator ShowNotification(string msg, float delay)
    {
        infoText.text = msg;
        infoText.gameObject.SetActive(true);
        // Используем WaitForSecondsRealtime, так как время может быть остановлено
        yield return new WaitForSecondsRealtime(delay);
        
        // Скрываем текст, если игрок еще не зашел в триггер победы
        if (canEscape && infoText.text != winMessage) 
            infoText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Сработает только если собрано 15 предметов и зашел Игрок
        if (other.CompareTag("Player") && canEscape)
        {
            StopAllCoroutines();
            
            if (infoText != null)
            {
                infoText.text = winMessage;
                infoText.gameObject.SetActive(true);
            }
            
            ScreenFader fader = FindObjectOfType<ScreenFader>();
            if (fader != null) fader.StartFade();

            Time.timeScale = 0f; 
        }
    }
}
