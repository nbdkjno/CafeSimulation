using UnityEngine;

public class TemporaryDrag : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 offset;
    private bool isDragging = true;

    void Start()
    {
        mainCamera = Camera.main;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        offset = transform.position - mousePos;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos + offset;
        }
    }

    void OnMouseUp()
    {
        // Останавливаем перетаскивание
        isDragging = false;
        // Удаляем временный скрипт
        Destroy(this);
    }
}