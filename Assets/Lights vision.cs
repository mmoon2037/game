using UnityEngine;
using UnityEngine.AI;

public class LightSeekerSimple : MonoBehaviour
{
    public float radius = 15f;
    public float angle = 90f;
    private NavMeshAgent agent;

    void Start() => agent = GetComponent<NavMeshAgent>();

    void Update()
    {
        // Находим все коллайдеры в радиусе
        Collider[] targets = Physics.OverlapSphere(transform.position, radius);
        Transform bestTarget = null;
        float closestDist = Mathf.Infinity;

        foreach (var t in targets)
        {
            // Проверяем тег
            if (t.CompareTag("Light"))
            {
                Vector3 dir = (t.transform.position - transform.position).normalized;

                // Проверка угла обзора
                if (Vector3.Angle(transform.forward, dir) < angle / 2f)
                {
                    float dist = Vector3.Distance(transform.position, t.transform.position);

                    // Проверка препятствий (Raycast)
                    // Теперь луч будет бить во всё подряд, поэтому проверяем, во что попали
                    if (Physics.Raycast(transform.position + Vector3.up, dir, out RaycastHit hit, dist))
                    {
                        // Если луч первым делом попал в свет (а не в стену)
                        if (hit.transform.CompareTag("Light"))
                        {
                            if (dist < closestDist)
                            {
                                closestDist = dist;
                                bestTarget = t.transform;
                            }
                        }
                    }
                }
            }
        }

        if (bestTarget != null) agent.SetDestination(bestTarget.position);
    }
}
