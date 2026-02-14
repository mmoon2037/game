using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    [Header("Настройки зрения")]
    public float viewDistance = 15f;
    public float viewAngle = 110f;
    public float searchTime = 3f; // Сколько секунд стоять после потери игрока

    private NavMeshAgent agent;
    private MonoBehaviour patrolScript; // Ссылка на ваш скрипт патруля
    private Transform player;
    private bool isSearching = false;
    private float searchTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Находим скрипт патруля (EnemyAI)
        patrolScript = GetComponent("EnemyAI") as MonoBehaviour;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            // Видим игрока: преследуем активно
            isSearching = false;
            patrolScript.enabled = false; 
            agent.isStopped = false;
            agent.speed = 5f; // Ускоряемся при погоне
            agent.destination = player.position;
        }
        else if (!patrolScript.enabled)
        {
            // Потеряли из виду: включаем логику поиска
            SearchLogic();
        }
    }

    void SearchLogic()
    {
        // Если еще не дошли до последней точки, где видели игрока
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (!isSearching)
            {
                isSearching = true;
                searchTimer = searchTime;
            }

            searchTimer -= Time.deltaTime;

            if (searchTimer <= 0f)
            {
                // Время вышло: возвращаемся к патрулю
                isSearching = false;
                agent.speed = 3.5f; // Возвращаем обычную скорость
                patrolScript.enabled = true;
            }
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - (transform.position + Vector3.up);
        float distance = directionToPlayer.magnitude;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (distance < viewDistance && angle < viewAngle / 2f)
        {
            if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer.normalized, out RaycastHit hit, viewDistance))
            {
                return hit.transform.CompareTag("Player");
            }
        }
        return false;
    }
}
