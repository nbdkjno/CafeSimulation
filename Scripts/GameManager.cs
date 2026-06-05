using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text mistakesText;
    public TMP_Text levelText;
    public GameObject gameOverPanel;

    [Header("Сложность")]
    public int difficultyLevel = 1;           // 1, 2 или 3
    public Image difficultyIcon;               // иконка уровня сложности
    public Sprite[] difficultySprites;         // [0]=уровень1, [1]=уровень2, [2]=уровень3

    [Header("Настройки сложности")]
    public float[] spawnDelays = { 4f, 3f, 2f };        // задержка спавна
    public float[] customerWaitTimes = { 20f, 15f, 10f }; // время клиента
    public int[] maxItemsPerOrder = { 1, 2, 2 };          // макс. предметов в заказе

    [Header("Спавн клиентов")]
    public GameObject customerPrefab;
    public Transform[] spawnPoints;      // 3 точки

    [Header("Игровые значения")]
    public int score = 0;
    public int mistakes = 0;
    public int maxMistakes = 3;
    public int currentLevel = 1;

    private bool isGameActive = true;
    private int ordersCompleted = 0;
    private int ordersForNextLevel = 3;

    // Следим, какие точки заняты
    private Dictionary<Transform, GameObject> occupiedSpots = new Dictionary<Transform, GameObject>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Загружаем сохранённую сложность (из меню)
        difficultyLevel = PlayerPrefs.GetInt("Difficulty", 1);
        UpdateDifficultyUI();

        UpdateUI();
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (isGameActive)
        {
            float currentSpawnDelay = spawnDelays[difficultyLevel - 1];
            yield return new WaitForSeconds(currentSpawnDelay);
            if (isGameActive)
                TrySpawnCustomer();
        }
    }

    void TrySpawnCustomer()
    {
        if (customerPrefab == null)
        {
            Debug.LogError("customerPrefab не назначен!");
            return;
        }

        // Находим все свободные точки
        List<Transform> freeSpots = new List<Transform>();
        foreach (Transform spot in spawnPoints)
        {
            if (!occupiedSpots.ContainsKey(spot) || occupiedSpots[spot] == null)
            {
                freeSpots.Add(spot);
            }
        }

        if (freeSpots.Count == 0)
        {
            Debug.Log("Все точки заняты, ждём...");
            return;
        }

        // Выбираем случайную свободную точку
        Transform selectedSpot = freeSpots[Random.Range(0, freeSpots.Count)];

        // Создаём клиента
        GameObject newCustomer = Instantiate(customerPrefab, selectedSpot.position, Quaternion.identity);

        // Запоминаем, что точка занята
        occupiedSpots[selectedSpot] = newCustomer;

        // Настраиваем клиента
        Customer customer = newCustomer.GetComponent<Customer>();
        if (customer != null)
        {
            customer.spawnPoint = selectedSpot;

            // Время ожидания клиента зависит от сложности
            customer.waitTime = customerWaitTimes[difficultyLevel - 1];

            // Сложность заказа зависит от уровня сложности
            int complexity = difficultyLevel;
            customer.SetOrder(complexity);
        }

        Debug.Log($"Клиент появился в точке {selectedSpot.name}");
    }

    public void FreeSpawnPoint(Transform spot)
    {
        if (occupiedSpots.ContainsKey(spot))
            occupiedSpots[spot] = null;
    }

    public void AddScore(int points)
    {
        if (!isGameActive) return;

        score += points;
        ordersCompleted++;

        if (ordersCompleted >= ordersForNextLevel)
        {
            ordersCompleted = 0;
            currentLevel++;
            Debug.Log($"Уровень повышен! Уровень: {currentLevel}");
        }

        UpdateUI();
    }

    public void AddMistake()
    {
        if (!isGameActive) return;

        mistakes++;
        UpdateUI();

        if (mistakes >= maxMistakes)
            GameOver();
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"Очки: {score}";
        if (mistakesText != null) mistakesText.text = $"Ошибки: {mistakes}/{maxMistakes}";
        if (levelText != null) levelText.text = $"Уровень: {currentLevel}";
    }

    void UpdateDifficultyUI()
    {
        if (difficultyIcon != null && difficultySprites != null && difficultySprites.Length >= 3)
        {
            difficultyIcon.sprite = difficultySprites[difficultyLevel - 1];
        }
    }

    // Метод для изменения сложности (вызывается из меню)
    public void SetDifficulty(int level)
    {
        difficultyLevel = Mathf.Clamp(level, 1, 3);
        PlayerPrefs.SetInt("Difficulty", difficultyLevel);
        PlayerPrefs.Save();
        UpdateDifficultyUI();
        Debug.Log($"Сложность изменена на уровень {difficultyLevel}");
    }

    void GameOver()
    {
        isGameActive = false;
        Time.timeScale = 0f;  // пауза
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Debug.Log($"Игра окончена! Очки: {score}");
    }

    public bool IsGameActive()
    {
        return isGameActive;
    }
}