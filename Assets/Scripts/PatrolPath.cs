using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float waitTime = 2f;
    
    private NavMeshAgent agent;
    private int currentPointIndex = 0;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToNextPoint();
    }

    void Update()
    {
        // Если скрипт активен и мы не ждем — патрулируем
        if (!isWaiting && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(WaitAndMove());
        }
    }

    IEnumerator WaitAndMove()
    {
        isWaiting = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(waitTime);
        
        currentPointIndex = (currentPointIndex + 1) % waypoints.Length;
        GoToNextPoint();
        
        agent.isStopped = false;
        isWaiting = false;
    }

    public void GoToNextPoint()
    {
        if (waypoints.Length > 0 && agent.isOnNavMesh) 
            agent.destination = waypoints[currentPointIndex].position;
    }
}
