using UnityEngine;

public class Cup : MonoBehaviour
{
    public bool isFull = false;
    public CoffeeMachine coffeeMachine;

    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;

    // Для временного отключения коллайдера кофемашины
    private Collider2D coffeeMachineCollider;
    private bool wasColliderEnabled = false;

    void Start()
    {
        mainCamera = Camera.main;

        if (coffeeMachine == null)
            coffeeMachine = FindFirstObjectByType<CoffeeMachine>();

        // Автоматически добавляем нужные компоненты
        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0;
        }

        if (GetComponent<BoxCollider2D>() == null)
            gameObject.AddComponent<BoxCollider2D>();

        // Сохраняем ссылку на коллайдер кофемашины
        if (coffeeMachine != null)
        {
            coffeeMachineCollider = coffeeMachine.GetComponent<Collider2D>();
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();

        // Отключаем коллайдер кофемашины, чтобы чашка не застревала
        if (coffeeMachineCollider != null && coffeeMachineCollider.enabled)
        {
            wasColliderEnabled = true;
            coffeeMachineCollider.enabled = false;
            Debug.Log("Коллайдер кофемашины ОТКЛЮЧЁН");
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
            transform.position = GetMouseWorldPos() + offset;
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Включаем коллайдер кофемашины обратно
        if (coffeeMachineCollider != null && wasColliderEnabled)
        {
            coffeeMachineCollider.enabled = true;
            wasColliderEnabled = false;
            Debug.Log("Коллайдер кофемашины ВКЛЮЧЁН");
        }

        if (!isFull && coffeeMachine != null)
        {
            float dist = Vector3.Distance(transform.position, coffeeMachine.transform.position);
            if (dist < 1.5f)
                coffeeMachine.TryMakeCoffee(gameObject);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }
}