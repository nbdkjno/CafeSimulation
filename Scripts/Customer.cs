using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Customer : MonoBehaviour
{
    [Header("Внешность")]
    public Sprite[] customerSprites;

    [Header("UI элементы")]
    public Transform orderPanel;
    public GameObject coffeeIconPrefab;
    public GameObject croissantIconPrefab;  // ← добавить
    public GameObject cakeIconPrefab;       // ← добавить
    public Image timerBar;

    [Header("Таймер")]
    public float waitTime = 15f;

    [Header("Системное")]
    public Transform spawnPoint;

    private float currentTime;
    private bool orderCompleted = false;
    private SpriteRenderer spriteRenderer;

    // Список того, что заказал клиент
    private List<string> requiredItems = new List<string>();
    private List<GameObject> orderIcons = new List<GameObject>();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (customerSprites != null && customerSprites.Length > 0 && spriteRenderer != null)
        {
            int randomIndex = Random.Range(0, customerSprites.Length);
            spriteRenderer.sprite = customerSprites[randomIndex];

            // Выравнивание по нижнему краю
            float spriteHeight = spriteRenderer.sprite.bounds.size.y;
            float groundLevel = -0.43f;  // ← уровень пола

            Vector3 pos = transform.position;
            pos.y = groundLevel + (spriteHeight / 2f);
            transform.position = pos;
        }

        currentTime = waitTime;
        if (timerBar != null)
            timerBar.fillAmount = 1f;

        StartCoroutine(TimerRoutine());
    }

    public void SetOrder(int complexityLevel)
    {
        Debug.Log($"Заказ создан. Сложность: {complexityLevel}");
        GenerateRandomOrder(complexityLevel);
    }

    void GenerateRandomOrder(int complexityLevel)
    {
        requiredItems.Clear();

        int maxItems;
        switch (complexityLevel)
        {
            case 1: maxItems = 1; break;      // лёгкий: 1 предмет
            case 2: maxItems = 2; break;      // средний: 1-2 предмета
            case 3: maxItems = 2; break;      // сложный: 2 предмета
            default: maxItems = 1; break;
        }

        int itemCount;
        if (complexityLevel == 1)
            itemCount = 1;
        else if (complexityLevel == 2)
            itemCount = Random.Range(1, 3); // 1 или 2
        else
            itemCount = 2; // всегда 2 предмета

        // Типы предметов
        string[] itemTypes = { "Coffee", "Croissant", "Cake" };

        for (int i = 0; i < itemCount; i++)
        {
            string randomItem = itemTypes[Random.Range(0, itemTypes.Length)];
            requiredItems.Add(randomItem);
        }

        Debug.Log($"Заказ: {string.Join(", ", requiredItems)}");
        ShowOrderIcons();
    }

    void ShowOrderIcons()
    {
        if (orderPanel == null)
        {
            Debug.LogError("Order Panel не назначен!");
            return;
        }

        // Очищаем старые иконки
        foreach (GameObject icon in orderIcons)
            Destroy(icon);
        orderIcons.Clear();

        // Создаём иконки для каждого заказанного предмета
        foreach (string item in requiredItems)
        {
            GameObject icon = null;

            if (item == "Coffee" && coffeeIconPrefab != null)
            {
                icon = Instantiate(coffeeIconPrefab, orderPanel);
            }
            else if (item == "Croissant" && croissantIconPrefab != null)
            {
                icon = Instantiate(croissantIconPrefab, orderPanel);
            }
            else if (item == "Cake" && cakeIconPrefab != null)
            {
                icon = Instantiate(cakeIconPrefab, orderPanel);
            }

            if (icon != null)
            {
                // Настройка размера иконки
                RectTransform rect = icon.GetComponent<RectTransform>();
                if (rect != null)
                    rect.sizeDelta = new Vector2(35, 35);

                orderIcons.Add(icon);
            }
        }
    }

    // Метод для проверки, нужен ли ещё этот предмет
    public bool NeedsItem(string itemType)
    {
        return requiredItems.Contains(itemType);
    }

    // Метод для получения предмета
    public void ReceiveItem(string itemType)
    {
        if (orderCompleted) return;

        if (requiredItems.Contains(itemType))
        {
            // Удаляем из списка первый найденный предмет
            int index = requiredItems.IndexOf(itemType);
            requiredItems.RemoveAt(index);

            // Удаляем соответствующую иконку (по тому же индексу)
            if (index < orderIcons.Count && orderIcons[index] != null)
            {
                Destroy(orderIcons[index]);
                orderIcons.RemoveAt(index);
            }

            Debug.Log($"Клиент получил {itemType}. Осталось: {requiredItems.Count}");

            if (requiredItems.Count == 0)
            {
                CompleteOrder();
            }
        }
    }

    void CompleteOrder()
    {
        orderCompleted = true;
        Debug.Log("КЛИЕНТ ПОЛУЧИЛ ВСЁ! +10 очков");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(10);
            GameManager.Instance.FreeSpawnPoint(spawnPoint);
        }

        FadeOut fade = GetComponent<FadeOut>();
        if (fade == null)
            fade = gameObject.AddComponent<FadeOut>();
        fade.StartFadeOut();
    }

    IEnumerator TimerRoutine()
    {
        while (currentTime > 0 && !orderCompleted)
        {
            currentTime -= Time.deltaTime;

            if (timerBar != null)
                timerBar.fillAmount = currentTime / waitTime;

            yield return null;
        }

        if (!orderCompleted)
        {
            Debug.Log("Клиент ушёл, не дождался!");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddMistake();
                GameManager.Instance.FreeSpawnPoint(spawnPoint);
            }
            FadeOut fade = GetComponent<FadeOut>();
            if (fade == null)
                fade = gameObject.AddComponent<FadeOut>();
            fade.StartFadeOut();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (orderCompleted) return;

        // Кофе
        Cup cup = other.GetComponent<Cup>();
        if (cup != null && cup.isFull)
        {
            ReceiveItem("Coffee");
            Destroy(other.gameObject);
            return;
        }

        // Круассан
        Croissant croissant = other.GetComponent<Croissant>();
        if (croissant != null && croissant.isReady)
        {
            ReceiveItem("Croissant");
            Destroy(other.gameObject);
            return;
        }

        // Торт
        Cake cake = other.GetComponent<Cake>();
        if (cake != null && cake.isReady)
        {
            ReceiveItem("Cake");
            Destroy(other.gameObject);
            return;
        }
    }
}