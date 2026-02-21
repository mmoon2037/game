using UnityEngine;

public class LightSwitcher : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private Light targetLight;
    [SerializeField] private KeyCode switchKey = KeyCode.F;

    void Awake()
    {
        // Ищем свет сначала на самом объекте, затем в дочерних
        if (targetLight == null)
        {
            targetLight = GetComponentInChildren<Light>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey) && targetLight != null)
        {
            targetLight.enabled = !targetLight.enabled;
        }
    }
}
