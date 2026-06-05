using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject rulesPanel;
    public GameObject gamePanel;

    [Header("Scene Names")]
    public string gameSceneName = "cafe12";  // Название сцены с игрой

    void Start()
    {
        ShowMainMenu();
    }

    // ---------- НАЖАТИЯ КНОПОК ----------
    public void OnPlayButtonClick()
    {
        Debug.Log("Нажата кнопка ИГРАТЬ! Загружаем сцену: " + gameSceneName);
        LoadGameScene();
    }

    public void OnRulesButtonClick()
    {
        Debug.Log("Нажата кнопка ПРАВИЛА!");
        ShowRules();
    }

    public void OnExitButtonClick()
    {
        Debug.Log("Нажата кнопка ВЫХОД!");
        ExitGame();
    }

    public void OnBackButtonClick()
    {
        Debug.Log("Нажата кнопка НАЗАД!");
        ShowMainMenu();
    }

    // ---------- ПЕРЕКЛЮЧЕНИЕ ПАНЕЛЕЙ ----------
    void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (rulesPanel != null) rulesPanel.SetActive(false);
        if (gamePanel != null) gamePanel.SetActive(false);
    }

    void ShowRules()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (rulesPanel != null) rulesPanel.SetActive(true);
        if (gamePanel != null) gamePanel.SetActive(false);
    }

    // ---------- ЗАГРУЗКА СЦЕНЫ С ИГРОЙ ----------
    void LoadGameScene()
    {
        // Проверяем, добавлена ли сцена в Build Settings
        if (IsSceneInBuild(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("Сцена '" + gameSceneName + "' не добавлена в Build Settings! " +
                           "Откройте File → Build Profiles и добавьте её в список Scenes In Build.");
        }
    }

    // Проверка, есть ли сцена в Build Settings
    bool IsSceneInBuild(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneNameFromPath == sceneName)
                return true;
        }
        return false;
    }

    // ---------- ВЫХОД ИЗ ИГРЫ ----------
    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}