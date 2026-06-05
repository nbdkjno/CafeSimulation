using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [Header("Аудио")]
    public AudioClip backgroundMusic;

    [Header("UI кнопка")]
    public Button musicButton;
    public Sprite musicOnSprite;   // ваша картинка "музыка включена"
    public Sprite musicOffSprite;  // ваша картинка "музыка выключена"

    private AudioSource audioSource;
    private bool isMusicOn = true;
    private Image buttonImage;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();

        buttonImage = musicButton.GetComponent<Image>();
        buttonImage.sprite = musicOnSprite;

        musicButton.onClick.AddListener(ToggleMusic);
    }

    void ToggleMusic()
    {
        isMusicOn = !isMusicOn;

        if (isMusicOn)
        {
            audioSource.Play();
            buttonImage.sprite = musicOnSprite;
        }
        else
        {
            audioSource.Stop();
            buttonImage.sprite = musicOffSprite;
        }
    }
}