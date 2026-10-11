using UnityEngine;
using DG.Tweening;

public class ClickRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 rotationAmount = new Vector3(0f, 0f, 10f);
    public float duration = 0.2f;
    public Ease ease = Ease.OutSine;

    private Quaternion originalRotation;
    private Tween rotationTween;

    void Awake()
    {
        originalRotation = transform.localRotation;
    }

    void OnMouseDown()
    {
        rotationTween?.Kill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalRotateQuaternion(
            originalRotation * Quaternion.Euler(rotationAmount),
            duration
        ).SetEase(ease));
        sequence.Append(transform.DOLocalRotateQuaternion(
            originalRotation,
            duration
        ).SetEase(Ease.OutBack));

        rotationTween = sequence;
    }

    void OnDisable()
    {
        rotationTween?.Kill();
        transform.localRotation = originalRotation;
    }

    void OnDestroy()
    {
        rotationTween?.Kill();
    }
}