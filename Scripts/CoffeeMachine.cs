using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    public Sprite fullJugSprite;
    public Sprite emptyJugSprite;
    public Sprite fullCupSprite;
    public Transform dispensePoint;

    public int maxCoffee = 5;
    public int currentCoffee = 5;

    public AudioClip pourSound;
    public ParticleSystem pourEffect;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        UpdateMachineSprite();
    }

    void UpdateMachineSprite()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.sprite = currentCoffee > 0 ? fullJugSprite : emptyJugSprite;
    }

    public bool TryMakeCoffee(GameObject cupObject)
    {
        if (currentCoffee <= 0)
        {
            Debug.Log("Нет кофе!");
            return false;
        }

        if (pourSound != null)
            audioSource.PlayOneShot(pourSound);

        if (pourEffect != null)
            pourEffect.Play();

        currentCoffee--;

        SpriteRenderer cupSr = cupObject.GetComponent<SpriteRenderer>();
        if (cupSr != null && fullCupSprite != null)
            cupSr.sprite = fullCupSprite;

        Cup cup = cupObject.GetComponent<Cup>();
        if (cup != null)
            cup.isFull = true;

        if (dispensePoint != null)
            cupObject.transform.position = dispensePoint.position;

        UpdateMachineSprite();
        Debug.Log($"Кофе налит! Осталось: {currentCoffee}/{maxCoffee}");
        return true;
    }

    // ========== ДОБАВИТЬ ЭТОТ МЕТОД ==========
    public void RefillCoffee(int amount)
    {
        currentCoffee += amount;
        if (currentCoffee > maxCoffee)
            currentCoffee = maxCoffee;

        UpdateMachineSprite();
        Debug.Log($"Кофе пополнен! Теперь: {currentCoffee}/{maxCoffee}");
    }
    // ========================================

    public void CupTaken() { }
}