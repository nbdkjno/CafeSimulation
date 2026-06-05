using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public string menuSceneName = "MenuScene";

    public void LoadMenuScene()
    {
        Debug.Log("Возвращаемся в меню!");
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    public void RestartGame()
    {
        Debug.Log("Перезапускаем игру!");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}