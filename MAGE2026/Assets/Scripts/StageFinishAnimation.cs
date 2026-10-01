using DG.Tweening;
using UnityEngine;

public class StageFinishAnimation : MonoBehaviour
{
    [Header("Paper")]
    public RectTransform paper;

    [Header("Animation")]
    public float startYOffset = 900f;
    public float startRotation = -12f;
    public float duration = 0.8f;
    public Ease ease = Ease.OutBack;

    private Vector2 originalPosition;
    private Quaternion originalRotation;
    private Tween positionTween;
    private Tween rotationTween;

    void Awake()
    {
        if (paper == null)
            return;

        originalPosition = paper.anchoredPosition;
        originalRotation = paper.localRotation;
    }

    public void Play()
    {
        if (paper == null)
        {
            Debug.LogError("StageCompleteAnimation: Paper is not assigned!");
            return;
        }

        positionTween?.Kill();
        rotationTween?.Kill();

        paper.anchoredPosition =
            originalPosition +
            Vector2.up * startYOffset;

        paper.localRotation =
            Quaternion.Euler(0f, 0f, startRotation);

        positionTween = paper
            .DOAnchorPos(originalPosition, duration)
            .SetEase(ease);

        rotationTween = paper
            .DOLocalRotate(
                originalRotation.eulerAngles,
                duration
            )
            .SetEase(ease);
    }

    public void Hide()
    {
        if (paper == null)
            return;

        positionTween?.Kill();
        rotationTween?.Kill();

        paper.anchoredPosition =
            originalPosition +
            Vector2.up * startYOffset;

        paper.localRotation =
            Quaternion.Euler(0f, 0f, startRotation);
    }
}