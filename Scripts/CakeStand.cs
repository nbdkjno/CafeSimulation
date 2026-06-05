using UnityEngine;

public class CakeStand : MonoBehaviour
{
    public GameObject cakePrefab;      // префаб куска торта
    public Sprite cakeSprite;          // спрайт торта (если нужно переопределить)

    private Camera mainCamera;
    private Collider2D standCollider;

    void Start()
    {
        mainCamera = Camera.main;
        standCollider = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        SpawnCake();
    }

    void SpawnCake()
    {
        if (cakePrefab == null) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        GameObject newCake = Instantiate(cakePrefab, mousePos, Quaternion.identity);

        // ≈сли нужно переопределить спрайт
        if (cakeSprite != null)
        {
            SpriteRenderer sr = newCake.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sprite = cakeSprite;
        }

        Cake cake = newCake.GetComponent<Cake>();
        if (cake != null)
        {
            cake.isReady = true;
        }

        if (standCollider != null)
            standCollider.enabled = false;

        Invoke("EnableCollider", 1.0f);

        Debug.Log(" усок торта вз€т!");
    }

    void EnableCollider()
    {
        if (standCollider != null)
            standCollider.enabled = true;
    }
}