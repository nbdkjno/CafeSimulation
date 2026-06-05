using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void GoToMenu()
    {
        Time.timeScale = 1f;  // на случай, если игра была на паузе
        SceneManager.LoadScene("Menu");
    }
}