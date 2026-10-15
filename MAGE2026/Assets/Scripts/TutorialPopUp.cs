using UnityEngine;
using DG.Tweening;

public class TutorialPopUp : MonoBehaviour
{
    [Header("Popup Animation")]
    public float popDuration = 0.35f;
    public Ease popEase = Ease.OutBack;
    private Transform popupTransform;
    private Vector3 originalScale;
    private Tween popTween;

    void Awake()
    {
        popupTransform = transform;
        originalScale = popupTransform.localScale;
    }

    public void ShowPopUp()
    {
        gameObject.SetActive(true);
        popTween?.Kill();
        popupTransform.localScale = Vector3.zero;
        popTween = popupTransform.DOScale(originalScale, popDuration).SetEase(popEase);
    }

    public void HidePopUp()
    {
        popTween?.Kill();
        popTween = null;
        gameObject.SetActive(false);
        popupTransform.localScale = originalScale;
    }

    void OnDestroy()
    {
        popTween?.Kill();
    }
}
