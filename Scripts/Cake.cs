using UnityEngine;

public class Cake : MonoBehaviour
{
    public bool isReady = true;

    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // Добавляем Rigidbody2D если нет
        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0;
        }

        // Добавляем Collider2D если нет
        if (GetComponent<BoxCollider2D>() == null)
        {
            BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
        }

        // Проверяем, что спрайт виден
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite == null)
        {
            Debug.LogError("У торта нет спрайта!");
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Торт: OnMouseDown сработал");
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    void OnMouseUp()
    {
        Debug.Log("Торт: OnMouseUp сработал");
        isDragging = false;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }
}