using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

// Memastikan objek memiliki AudioSource
[RequireComponent(typeof(AudioSource))]
public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Hover Scale")]
    public float hoverScaleMultiplier = 1.15f;
    public float hoverScaleDuration = 0.2f;

    [Header("Audio Settings")]
    public AudioClip hoverSound;
    public AudioClip clickSound;
    
    private Vector3 originalScale;
    private Tween scaleTween;
    private AudioSource audioSource;

    void Awake()
    {
        originalScale = transform.localScale;
        // Mengambil referensi AudioSource saat objek dibuat
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        scaleTween?.Kill();
        scaleTween = transform.DOScale(originalScale * hoverScaleMultiplier, hoverScaleDuration).SetEase(Ease.OutQuad);
        
        // Memainkan suara hover
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scaleTween?.Kill();
        scaleTween = transform.DOScale(originalScale, hoverScaleDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Memainkan suara klik
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    void OnDisable()
    {
        scaleTween?.Kill();
        transform.localScale = originalScale;
    }

    void OnDestroy()
    {
        scaleTween?.Kill();
    }
}