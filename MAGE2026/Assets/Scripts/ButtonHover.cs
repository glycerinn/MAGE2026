using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Scale")]
    public float hoverScaleMultiplier = 1.15f;
    public float hoverScaleDuration = 0.2f;

    private Vector3 originalScale;
    private Tween scaleTween;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        scaleTween?.Kill();
        scaleTween = transform.DOScale(originalScale * hoverScaleMultiplier, hoverScaleDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scaleTween?.Kill();
        scaleTween = transform.DOScale(originalScale, hoverScaleDuration).SetEase(Ease.OutQuad);
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