using UnityEngine;
using DG.Tweening;

public class DialogueBob : MonoBehaviour
{
    [Header("Bob Animation")]
    public float bobHeight = 12f;
    public float bobDuration = 0.6f;
    public Ease bobEase = Ease.InOutSine;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Tween bobTween;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        bobTween?.Kill();

        rectTransform.anchoredPosition = originalPosition;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(
            rectTransform.DOAnchorPosY(
                originalPosition.y + bobHeight,
                bobDuration
            ).SetEase(bobEase)
        );
        sequence.Append(
            rectTransform.DOAnchorPosY(
                originalPosition.y,
                bobDuration
            ).SetEase(bobEase)
        );
        sequence.SetLoops(-1, LoopType.Restart);

        bobTween = sequence;
    }

    public void Hide()
    {
        bobTween?.Kill();
        bobTween = null;

        if (rectTransform != null)
            rectTransform.anchoredPosition = originalPosition;

        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        bobTween?.Kill();
        bobTween = null;
    }

    void OnDestroy()
    {
        bobTween?.Kill();
    }
}