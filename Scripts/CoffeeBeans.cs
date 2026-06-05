using UnityEngine;
using System.Collections;

public class CoffeeBeans : MonoBehaviour
{
    [Header("Настройки пополнения")]
    public int refillAmount = 3;        // сколько порций добавляет пачка

    [Header("Эффекты")]
    public AudioClip refillSound;       // звук пополнения
    public ParticleSystem refillEffect; // эффект (вспышка)

    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 startPosition;      // начальная позиция
    private Quaternion startRotation;   // начальный поворот
    private Camera mainCamera;
    private CoffeeMachine coffeeMachine;
    private AudioSource audioSource;

    void Start()
    {
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // ЗАПОМИНАЕМ начальное положение
        startPosition = transform.position;
        startRotation = transform.rotation;

        // Находим кофемашину
        coffeeMachine = FindAnyObjectByType<CoffeeMachine>();

        if (coffeeMachine == null)
            Debug.LogError("Кофемашина не найдена на сцене!");
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Проверяем, положили ли пачку на кофемашину
        if (coffeeMachine != null)
        {
            float distance = Vector3.Distance(transform.position, coffeeMachine.transform.position);

            if (distance < 1.5f)
            {
                // Пополняем кофе
                coffeeMachine.RefillCoffee(refillAmount);

                // Звук
                if (refillSound != null)
                    audioSource.PlayOneShot(refillSound);

                // Эффект
                if (refillEffect != null)
                {
                    ParticleSystem effect = Instantiate(refillEffect, coffeeMachine.transform.position, Quaternion.identity);
                    effect.Play();
                    Destroy(effect.gameObject, 1f);
                }

                Debug.Log($"Кофе пополнен! +{refillAmount}");
            }
        }

        // ВСЕГДА возвращаем пачку на место (и после пополнения, и если просто бросили)
        ReturnToStart();
    }

    void ReturnToStart()
    {
        // Останавливаем перетаскивание
        isDragging = false;

        // Возвращаем объект в начальную позицию
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }
}