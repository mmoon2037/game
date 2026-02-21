using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    [Header("Stamina Logic")]
    public float maxStamina = 5f;
    public float currentStamina;
    public float staminaDepletionRate = 1f;
    public float staminaRegenRate = 0.5f;
    [Range(0, 1)] public float staminaThreshold = 0.2f;

    [Header("Stamina UI Custom")]
    public Slider staminaSlider;
    public Image fillImage;        // Перетащи сюда картинку Fill
    public CanvasGroup uiGroup;    // Перетащи сюда Canvas Group (для прозрачности)
    public Color normalColor = Color.green;
    public Color exhaustedColor = Color.red;

    private bool isExhausted = false;
    Rigidbody rigidbody;
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentStamina = maxStamina;

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = maxStamina;
        }
    }

    void Update()
    {
        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        if (currentStamina <= 0) isExhausted = true;
        if (isExhausted && currentStamina >= maxStamina * staminaThreshold) isExhausted = false;

        bool tryingToRun = Input.GetKey(runningKey) && canRun && isMoving && !isExhausted;

        if (tryingToRun && currentStamina > 0)
            currentStamina -= staminaDepletionRate * Time.deltaTime;
        else
            currentStamina += staminaRegenRate * Time.deltaTime;

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (staminaSlider == null) return;

        // 1. Плавное обновление значения
        staminaSlider.value = Mathf.Lerp(staminaSlider.value, currentStamina, Time.deltaTime * 10);

        // 2. Управление цветом
        if (fillImage != null)
            fillImage.color = Color.Lerp(fillImage.color, isExhausted ? exhaustedColor : normalColor, Time.deltaTime * 5);

        // 3. Скрытие UI, когда стамина полная (через CanvasGroup)
        if (uiGroup != null)
        {
            float targetAlpha = (currentStamina >= maxStamina) ? 0 : 1;
            uiGroup.alpha = Mathf.MoveTowards(uiGroup.alpha, targetAlpha, Time.deltaTime * 2);
        }
    }

    void FixedUpdate()
    {
        IsRunning = canRun && Input.GetKey(runningKey) && currentStamina > 0.1f && !isExhausted;
        float targetMovingSpeed = IsRunning ? runSpeed : speed;

        if (speedOverrides.Count > 0)
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();

        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
        rigidbody.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.linearVelocity.y, targetVelocity.y);
    }
}
