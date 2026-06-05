using UnityEngine;

public class CroissantStand : MonoBehaviour
{
    public GameObject croissantPrefab;
    public Sprite croissantSprite;

    private Camera mainCamera;
    private Collider2D standCollider;

    void Start()
    {
        mainCamera = Camera.main;
        standCollider = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        SpawnCroissant();
    }

    void SpawnCroissant()
    {
        if (croissantPrefab == null) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        GameObject newCroissant = Instantiate(croissantPrefab, mousePos, Quaternion.identity);

        if (croissantSprite != null)
        {
            SpriteRenderer sr = newCroissant.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sprite = croissantSprite;
        }

        Croissant croissant = newCroissant.GetComponent<Croissant>();
        if (croissant != null)
        {
            croissant.isReady = true;
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