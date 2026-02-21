using UnityEngine;

public class Stamina : MonoBehaviour
{
    [Header("Settings")]
    public float maxStamina = 5f; // Стамина на 5 секунд
    public float currentStamina;
    public float regenerationRate = 1f; // Скорость восстановления
    public float consumptionRate = 1f;  // Скорость траты

    [Header("Speed Settings")]
    public float staminaRunSpeed = 12f; // Особая скорость при беге со стаминой

    private FirstPersonMovement movement;

    void Awake()
    {
        movement = GetComponent<FirstPersonMovement>();
        currentStamina = maxStamina;

        // Добавляем override скорости в список движений
        movement.speedOverrides.Add(GetStaminaSpeed);
    }

    void Update()
    {
        // Если игрок нажал кнопку бега и движение сообщает, что мы МОЖЕМ бежать
        bool isTryingToRun = Input.GetKey(movement.runningKey) && movement.canRun;

        if (isTryingToRun && currentStamina > 0)
        {
            // Тратим стамину
            currentStamina -= consumptionRate * Time.deltaTime;
        }
        else
        {
            // Восстанавливаем стамину
            currentStamina += regenerationRate * Time.deltaTime;
        }

        // Ограничиваем значения
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    // Эта функция будет вызываться внутри FirstPersonMovement
    float GetStaminaSpeed()
    {
        // Если зажат шифт и есть хоть немного стамины — даем бонусную скорость
        if (Input.GetKey(movement.runningKey) && currentStamina > 0.1f)
        {
            return staminaRunSpeed;
        }

        // Если стамины нет или шифт не зажат, возвращаем обычную скорость
        return movement.speed;
    }
}
