using System;
using TMPro;
using UnityEngine;

public class grab : MonoBehaviour
{
    public GameObject textobject;
    private TextMeshProUGUI counter;

    void Start()
    {
        if (textobject != null)
            counter = textobject.GetComponent<TextMeshProUGUI>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int currentCount = Int32.Parse(counter.text) + 1;
            counter.SetText(currentCount.ToString());

            // Активируем выход строго при достижении 15
            if (currentCount >= 2)
            {
                EscapeZone escape = FindObjectOfType<EscapeZone>(true);
                if (escape != null)
                {
                    escape.ActivateEscape(); // Вызываем метод активации
                }
            }

            Destroy(gameObject);
        }
    }
}
