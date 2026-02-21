using UnityEngine;
using System.Collections;

public class SmoothDoor : MonoBehaviour
{
    public float speed = 3f;          // Скорость движения
    public float closeDelay = 2f;     // Задержка закрытия (2 секунды)

    private Vector3 closedPos;
    private Vector3 openedPos;
    private bool isMoving = false;    // Чтобы не запускать много раз сразу

    void Start()
    {
        closedPos = transform.position;
        // Чтобы поднялась "в два раза" (на свою высоту + еще одну), 
        // используем размер коллайдера или scale по Y
        float height = GetComponent<Collider>().bounds.size.y;
        openedPos = closedPos + Vector3.up * (height * 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Если коснулся игрок или враг и дверь сейчас не в движении
        if (!isMoving && (other.CompareTag("Player") || other.CompareTag("Enemy")))
        {
            StartCoroutine(DoorCycle());
        }
    }

    IEnumerator DoorCycle()
    {
        isMoving = true;

        // Плавно вверх
        yield return MoveDoor(openedPos);

        // Ждем 2 секунды
        yield return new WaitForSeconds(closeDelay);

        // Плавно вниз
        yield return MoveDoor(closedPos);

        isMoving = false;
    }

    IEnumerator MoveDoor(Vector3 target)
    {
        // Используем MoveTowards для более равномерного движения
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }
}
