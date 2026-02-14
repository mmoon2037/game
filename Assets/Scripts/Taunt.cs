using UnityEngine;
using TMPro; 
using UnityEngine.AI;
using System.Collections;

public class EnemyTaunt : MonoBehaviour
{
    [Header("Настройки времени")]
    public float tauntInterval = 30f;
    public float textDuration = 3f;

    [Header("Ссылки")]
    public TextMeshProUGUI tauntText; 
    public string message = "Я ИДУ ЗА ТОБОЙ...";

    private NavMeshAgent agent;
    private Transform player;
    private EnemyAI patrolScript;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        patrolScript = GetComponent<EnemyAI>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (tauntText != null) tauntText.gameObject.SetActive(false);
        timer = tauntInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ExecuteTaunt();
            timer = tauntInterval;
        }
    }

    void ExecuteTaunt()
    {
        if (tauntText != null)
        {
            StartCoroutine(ShowText());
        }

        patrolScript.enabled = false;
        agent.isStopped = false;
        agent.destination = player.position;
        
        StartCoroutine(ReturnToPatrolAfterArrival());
    }

    IEnumerator ShowText()
    {
        tauntText.text = message;
        tauntText.gameObject.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        tauntText.gameObject.SetActive(false);
    }

    IEnumerator ReturnToPatrolAfterArrival()
    {
        yield return new WaitForSeconds(1f); // Даем время на начало пути
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.6f);
        
        patrolScript.enabled = true;
    }
}
