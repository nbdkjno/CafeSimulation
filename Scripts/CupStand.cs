using UnityEngine;

public class CupStand : MonoBehaviour
{
    public GameObject emptyCupPrefab;
    public Sprite emptyCupSprite;

    private Camera mainCamera;
    private Collider2D standCollider;

    void Start()
    {
        mainCamera = Camera.main;
        standCollider = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        SpawnCup();
    }

    void SpawnCup()
    {
        if (emptyCupPrefab == null || emptyCupSprite == null) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        GameObject newCup = Instantiate(emptyCupPrefab, mousePos, Quaternion.identity);

        SpriteRenderer sr = newCup.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = emptyCupSprite;

        Cup cup = newCup.GetComponent<Cup>();
        if (cup != null)
        {
            cup.isFull = false;
            cup.coffeeMachine = FindFirstObjectByType<CoffeeMachine>();
        }

        // Отключаем коллайдер подставки
        if (standCollider != null)
            standCollider.enabled = false;
        Invoke("EnableCollider", 1.0f);
    }

    void EnableCollider()
    {
        if (standCollider != null)
            standCollider.enabled = true;
    }
}