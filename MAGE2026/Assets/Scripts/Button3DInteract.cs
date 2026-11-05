using UnityEngine;

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

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * smoothSpeed);
    }

    void OnMouseEnter()
    {
        targetScale = originalScale * hoverScaleMultiplier;
        
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