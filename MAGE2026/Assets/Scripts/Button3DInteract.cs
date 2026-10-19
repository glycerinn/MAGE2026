using UnityEngine;

// Baris ini memastikan Unity otomatis menambahkan komponen AudioSource ke objek Anda
[RequireComponent(typeof(AudioSource))]
public class Button3DInteract : MonoBehaviour
{
    private Vector3 originalScale;
    private Vector3 targetScale;
    
    [Header("Settings - Animation")]
    public float hoverScaleMultiplier = 1.1f;
    public float clickScaleMultiplier = 0.95f;
    public float smoothSpeed = 15f;

    [Header("Settings - Audio")]
    public AudioClip hoverSound;
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        // Mengambil referensi AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Transisi halus menggunakan Lerp
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * smoothSpeed);
    }

    void OnMouseEnter()
    {
        targetScale = originalScale * hoverScaleMultiplier;
        
        // Memainkan suara saat di-hover
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    void OnMouseExit()
    {
        targetScale = originalScale;
    }

    void OnMouseDown()
    {
        targetScale = originalScale * clickScaleMultiplier;
        
        // Memainkan suara saat diklik
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    void OnMouseUp()
    {
        targetScale = originalScale * hoverScaleMultiplier;
    }
}