using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    // Перетащи сюда объект, который хочешь скрывать/показывать
    public GameObject targetObject;

    void Update()
    {
        // Проверяем нажатие клавиши 1 (не на нампаде, а в верхнем ряду)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (targetObject != null)
            {
                // Инвертируем текущее состояние (включен -> выключен и наоборот)
                bool isActive = targetObject.activeSelf;
                targetObject.SetActive(!isActive);
            }
        }
    }
}
