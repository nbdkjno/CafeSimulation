using UnityEngine;
using TMPro;  // ← добавить эту строку
using UnityEngine.UI;

public class InGameDifficulty : MonoBehaviour
{
    private TMP_Dropdown dropdown;  // ← изменить на TMP_Dropdown
    private GameManager gameManager;

    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();  // ← изменить на TMP_Dropdown
        if (dropdown == null)
        {
            Debug.LogError("TMP_Dropdown компонент не найден!");
            return;
        }

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager не найден!");
            return;
        }

        int savedDifficulty = PlayerPrefs.GetInt("Difficulty", 1);
        dropdown.value = savedDifficulty - 1;
        gameManager.SetDifficulty(savedDifficulty);
        dropdown.onValueChanged.AddListener(OnDifficultyChanged);
        dropdown.RefreshShownValue();

        Debug.Log("InGameDifficulty инициализирован");
    }

    void OnDifficultyChanged(int index)
    {
        int newDifficulty = index + 1;
        PlayerPrefs.SetInt("Difficulty", newDifficulty);
        PlayerPrefs.Save();

        if (gameManager != null)
            gameManager.SetDifficulty(newDifficulty);

        Debug.Log($"Сложность изменена на: {newDifficulty}");
    }
}